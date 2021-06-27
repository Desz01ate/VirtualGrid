using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Tests.Adapters
{
    class TestAdapter : IPhysicalDeviceAdapter
    {
        public string Name => "";

        public bool Initialized => true;

        public int RowCount => 1000;

        public int ColumnCount => 1000;

        public Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
