using Colore;
using Colore.Effects.ChromaLink;
using Colore.Effects.Headset;
using Colore.Effects.Keyboard;
using Colore.Effects.Mouse;
using Colore.Effects.Mousepad;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Enums;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible keyboard effects.
    /// </summary>
    public class RazerKeyboardAdapter : RazerPeripheralBaseAdapter
    {
        //<inheritdoc/>
        public override string Name => "RΛZΞR Chroma";

        public override int RowCount => 6;

        public override int ColumnCount => 22;

        public override async Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
                return;

            var keyboardGrid = CustomKeyboardEffect.Create();

            for (var row = 0; row < virtualGrid.RowCount; row++)
            {
                for (var col = 0; col < virtualGrid.ColumnCount; col++)
                {
                    keyboardGrid[row, col] = ToColoreColor(virtualGrid[col, row]);
                }
            }

            await this.ChromaInterface.Keyboard.SetCustomAsync(keyboardGrid);
        }
    }
}
