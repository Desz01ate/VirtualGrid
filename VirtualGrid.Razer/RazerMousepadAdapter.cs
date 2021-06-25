using Colore;
using Colore.Effects.Mousepad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible mousepad effects.
    /// </summary>
    public class RazerMousepadAdapter : RazerPeripheralBaseAdapter
    {
        //<inheritdoc/>
        public override string Name => "RΛZΞR Mousepad";

        public override int RowCount => 1;

        public override int ColumnCount => 15;

        public override async Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
                return;

            var mousepadGrid = CustomMousepadEffect.Create();

            var keyIdx = 0;
            foreach (var key in virtualGrid.Reverse())
            {
                mousepadGrid[keyIdx++] = ToColoreColor(key.Color);
            }

            await this.ChromaInterface.Mousepad.SetCustomAsync(mousepadGrid);
        }
    }
}
