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
        private readonly ICollection<IPhysicalDeviceAdapter> _adapters;
        private readonly IVirtualLedGrid _grid;

        private bool _disposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="grid">An instance of virtual grid.</param>
        public PhysicalDeviceMediator(IVirtualLedGrid grid)
        {
            this._grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this._adapters = new List<IPhysicalDeviceAdapter>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="grid">An instance of virtual grid.</param>
        /// <param name="adapters">An instance of collection of physical device adapters.</param>
        public PhysicalDeviceMediator(IVirtualLedGrid grid, ICollection<IPhysicalDeviceAdapter> adapters)
        {
            this._grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this._adapters = adapters ?? new List<IPhysicalDeviceAdapter>();
        }

        /// <summary>
        /// Attach physical device adapter to mediator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Attach<T>() where T : IPhysicalDeviceAdapter, new()
        {
            T adapter = new();
            Attach(adapter);
        }

        /// <summary>
        /// Attach physical device adapter to mediator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Attach(IPhysicalDeviceAdapter adapter)
        {
            if (adapter == null)
                throw new ArgumentNullException(nameof(_adapters));
            if (!adapter.Initialized)
                return;

            this._adapters.Add(adapter);
        }

        /// <summary>
        /// Apply rendered grid to physical devices.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ApplyAsync(CancellationToken cancellationToken = default)
        {
            if (!_grid.Any())
                return Task.CompletedTask;

            var tasks = new Task[_adapters.Count];
            for (var idx = 0; idx < _adapters.Count; idx++)
            {
                var adapter = _adapters.ElementAt(idx);
                var task = adapter.ApplyAsync(this._grid, cancellationToken);
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
                foreach (var adapter in this._adapters)
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
