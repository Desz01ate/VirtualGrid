using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid
{
    /// <summary>
    /// A mediator to arrange task between virtual grids and physical devices.
    /// </summary>
    public sealed class PhysicalDeviceMediator : IDeviceArrangeMediator
    {
        private readonly IDictionary<IPhysicalDeviceAdapter, (int X, int Y)> _adapters;
        private readonly ICollection<IVirtualLedGrid> _grids;
        private readonly Channel<(IVirtualLedGrid Grid, IPhysicalDeviceAdapter Adapter)> _channel;

        private bool _disposed;

        /// <inheritdoc/>
        public IEnumerable<IVirtualLedGrid> AttachedLedGrids => _grids;

        /// <inheritdoc/>
        public IEnumerable<IPhysicalDeviceAdapter> AttachedAdapters => _adapters.Keys;

        /// <summary>
        /// Constructor for physical device mediator with single virtual LED grid.
        /// </summary>
        /// <param name="grid"></param>
        public PhysicalDeviceMediator(IVirtualLedGrid grid) : this(new[] { grid })
        {
        }

        /// <summary>
        /// Constructor for physical device mediator with multiple virtual LED grids.
        /// </summary>
        /// <param name="grids"></param>
        public PhysicalDeviceMediator(params IVirtualLedGrid[] grids)
        {
            if (grids == null || !grids.Any())
            {
                throw new ArgumentNullException(nameof(grids));
            }

            if (grids.Any(x => x == null))
            {
                throw new ArgumentException(nameof(grids));
            }

            this._grids = grids;
            this._adapters = new Dictionary<IPhysicalDeviceAdapter, (int X, int Y)>();
            this._channel = Channel.CreateBounded<(IVirtualLedGrid Grid, IPhysicalDeviceAdapter Adapter)>(100);

            Task.Run(() => this.ConsumeAsync());
        }

        /// <inheritdoc/>
        public void Attach<T>(int x, int y) where T : IPhysicalDeviceAdapter, new()
        {
            T adapter = new();
            Attach(x, y, adapter);
        }

        /// <inheritdoc/>
        public void Attach(int x, int y, IPhysicalDeviceAdapter adapter)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentOutOfRangeException($"Attach range must be at positive index.");
            }

            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(_adapters));
            }

            if (!adapter.Initialized)
            {
                return;
            }

            var adapterType = adapter.GetType();

            if (this._adapters.Any(x => x.Key.GetType() == adapterType))
            {
                throw new InvalidOperationException("Unable to attach the same adapter type to the mediator.");
            }

            this._adapters.Add(adapter, (x, y));
        }

        /// <inheritdoc/>
        public bool UpdateAdapterPosition<T>(int x, int y) where T : IPhysicalDeviceAdapter
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentOutOfRangeException($"Attach range must be at positive index.");
            }

            var adapter = this._adapters.SingleOrDefault(x => x.Key.GetType() == typeof(T));

            if (adapter.Key == null)
            {
                return false;
            }

            this._adapters[adapter.Key] = (x, y);

            return true;
        }

        /// <inheritdoc/>
        public IPhysicalDeviceAdapter? Detach<T>() where T : IPhysicalDeviceAdapter
        {
            var adapter = this._adapters.SingleOrDefault(x => x.Key.GetType() == typeof(T)).Key;

            if (adapter == null)
            {
                return null;
            }

            this._adapters.Remove(adapter);

            return adapter;
        }

        /// <inheritdoc/>
        public async Task ApplyAsync(CancellationToken cancellationToken = default)
        {
            var grid = this._grids.Aggregate((x, y) => x + y);

            if (!grid.Any())
            {
                return;
            }

            for (var idx = 0; idx < _adapters.Count; idx++)
            {
                var adapterPair = _adapters.ElementAt(idx);
                var adapter = adapterPair.Key;
                var (X, Y) = adapterPair.Value;
                var slicedGrid = grid.Slice(X, Y, adapter.ColumnCount, adapter.RowCount);

                if (slicedGrid == null)
                {
                    continue;
                }

                await this._channel.Writer.WriteAsync((slicedGrid, adapter), cancellationToken);
            }
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var adapter in this._adapters.Keys)
                {
                    adapter.Dispose();
                }
            }

            this._disposed = true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task ConsumeAsync()
        {
            var reader = this._channel.Reader;

            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var message))
                {
                    var grid = message.Grid;
                    var adapter = message.Adapter;

                    await adapter.ApplyAsync(grid);
                }
            }
        }
    }
}
