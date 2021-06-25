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
        public override string Name => "RΛZΞR Chroma";

        public override int RowCount => 1;

        public override int ColumnCount => 3;

        public override async Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
                return;

            var headset = CustomHeadsetEffect.Create();

            var actualIdx = 0;
            var keyIdx = 0;
            foreach (var key in virtualGrid)
            {
                if (keyIdx++ % 2 != 0) //create a gap between L and R
                    continue;

                headset[actualIdx++] = ToColoreColor(key.Color);
            }

            await this.ChromaInterface.Headset.SetCustomAsync(headset);
        }
    }
}
