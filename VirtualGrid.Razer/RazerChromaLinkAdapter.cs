﻿using Colore.Effects.ChromaLink;
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

        public override Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
            {
                return Task.CompletedTask;
            }

            var chromaLinkGrid = CustomChromaLinkEffect.Create();

            var keyIdx = 0;
            foreach (var key in virtualGrid)
            {
                chromaLinkGrid[keyIdx++] = ToColoreColor(key.Color);
            }

            return this.ChromaInterface!.ChromaLink.SetCustomAsync(chromaLinkGrid);
        }
    }
}
