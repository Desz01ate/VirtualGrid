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
        /// <summary>
        /// Razer Chroma external connector interface.
        /// </summary>
        protected readonly IChroma? ChromaInterface;

        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public bool Initialized { get; private set; }

        /// <inheritdoc/>
        public abstract int RowCount { get; }

        /// <inheritdoc/>
        public abstract int ColumnCount { get; }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Dispose()
        {
            this.ChromaInterface?.Dispose();
        }

        /// <summary>
        /// Convert <see cref="VirtualGrid.Color"/> to <see cref="Colore.Data.Color"/> which is supported by Razer SDK.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        protected static Colore.Data.Color ToColoreColor(Color color)
        {
            return new Colore.Data.Color(color.Value);
        }
    }
}
