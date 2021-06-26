using Colore.Effects.Headset;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible headset effects.
    /// </summary>
    public class RazerHeadsetAdapter : RazerPeripheralBaseAdapter
    {
        /// <inheritdoc/>
        public override string Name => "RΛZΞR Headset";

        /// <inheritdoc/>
        public override int RowCount => 1;

        /// <inheritdoc/>
        public override int ColumnCount => 3;

        /// <inheritdoc/>
        public override async Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
                return;

            var headset = CustomHeadsetEffect.Create();

            var actualIdx = 0;
            var keyIdx = 0;
            foreach (var cell in virtualGrid)
            {
                if (keyIdx++ % 2 != 0) //create a gap between L and R
                    continue;
                if (cell.Color == null)
                    continue;

                headset[actualIdx++] = ToColoreColor(cell.Color.Value);
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            await this.ChromaInterface.Headset.SetCustomAsync(headset);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        }
    }
}
