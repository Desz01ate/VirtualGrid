using Colore.Effects.ChromaLink;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible ChromaLink effects.
    /// </summary
    public class RazerChromaLinkAdapter : RazerPeripheralBaseAdapter
    {
        public override string Name => "RΛZΞR ChromaLink";

        public override int RowCount => 1;

        public override int ColumnCount => 5;

        public override async Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
                return;

            var chromaLinkGrid = CustomChromaLinkEffect.Create();

            var keyIdx = 0;
            foreach (var key in virtualGrid)
            {
                chromaLinkGrid[keyIdx++] = ToColoreColor(key.Color);
            }

            await this.ChromaInterface.ChromaLink.SetCustomAsync(chromaLinkGrid);
        }
    }
}
