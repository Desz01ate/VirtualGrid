using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid
{
    /// <summary>
    /// A mediator to arrange task between virtual grid and physical devices.
    /// </summary>
    public sealed class PhysicalDeviceMediator : IArrangeMediator
    {
        private readonly IDictionary<IPhysicalDeviceAdapter, (int X, int Y)> _adapters;
        private readonly IVirtualLedGrid _grid;

        private bool _disposed;

        public PhysicalDeviceMediator(IVirtualLedGrid grid)
        {
            this._grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this._adapters = new Dictionary<IPhysicalDeviceAdapter, (int X, int Y)>();
        }

        public void Attach<T>(int x, int y) where T : IPhysicalDeviceAdapter, new()
        {
            T adapter = new();
            Attach(x, y, adapter);
        }

        public void Attach(int x, int y, IPhysicalDeviceAdapter adapter)
        {
            if (x < 0 || y < 0)
                throw new ArgumentOutOfRangeException($"Attach range must be at positive index.");
            if (adapter == null)
                throw new ArgumentNullException(nameof(_adapters));
            if (!adapter.Initialized)
                return;

            this._adapters.Add(adapter, (x, y));
        }

        public bool UpdateAdapterPosition<T>(int x, int y) where T : IPhysicalDeviceAdapter
        {
            if (x < 0 || y < 0)
                throw new ArgumentOutOfRangeException($"Attach range must be at positive index.");

            var adapter = this._adapters.SingleOrDefault(x => x.GetType() == typeof(T));
            if (adapter.Key == null)
            {
                return false;
            }

            this._adapters[adapter.Key] = (x, y);
            return true;
        }

        public IPhysicalDeviceAdapter? Detach<T>() where T : IPhysicalDeviceAdapter
        {
            var adapter = this._adapters.SingleOrDefault(x => x.GetType() == typeof(T)).Key;
            if (adapter == null)
            {
                return null;
            }

            this._adapters.Remove(adapter);
            return adapter;
        }

        public Task ApplyAsync(CancellationToken cancellationToken = default)
        {
            if (!_grid.Any())
                return Task.CompletedTask;

            var tasks = new Task[_adapters.Count];
            for (var idx = 0; idx < _adapters.Count; idx++)
            {
                var adapterPair = _adapters.ElementAt(idx);
                var adapter = adapterPair.Key;
                var (X, Y) = adapterPair.Value;
                var slicedGrid = _grid.Slice(X, Y, adapter.ColumnCount, adapter.RowCount);
                var task = adapter.ApplyAsync(slicedGrid, cancellationToken);
                tasks[idx] = task;
            }
            return Task.WhenAll(tasks);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed)
                return;

            if (disposing)
            {
                foreach (var adapter in this._adapters.Keys)
                {
                    adapter.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
