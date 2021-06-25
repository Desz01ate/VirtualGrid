using Colore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An abstract adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible effects.
    /// For more information about Razer Chroma LED profile : https://developer.razer.com/works-with-chroma/razer-chroma-led-profiles/
    /// </summary>
    public abstract class RazerPeripheralBaseAdapter : IPhysicalDeviceAdapter
    {
        protected readonly IChroma? ChromaInterface;
        public abstract string Name { get; }

        public bool Initialized { get; private set; }

        public abstract int RowCount { get; }

        public abstract int ColumnCount { get; }

        public abstract Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default);

        internal RazerPeripheralBaseAdapter()
        {
            try
            {
                this.ChromaInterface = ChromaConnectorInterfaceSingleton.ChromaConnector;
                this.Initialized = true;
            }
            catch (Exception)
            {
                this.Initialized = false;
            }
        }

        public void Dispose()
        {
            this.ChromaInterface?.Dispose();
        }

        protected static Colore.Data.Color ToColoreColor(Color color)
        {
            return new Colore.Data.Color(color.Value);
        }
    }
}
