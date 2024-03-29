﻿using Colore.Effects.Keyboard;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible keyboard effects.
    /// </summary>
    public class RazerKeyboardAdapter : RazerPeripheralBaseAdapter
    {
        public override string Name => "RΛZΞR Keyboard";

        public override int RowCount => 6;

        public override int ColumnCount => 22;

        public override Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
            {
                return Task.CompletedTask;
            }

            var keyboardGrid = CustomKeyboardEffect.Create();

            for (var row = 0; row < virtualGrid.RowCount; row++)
            {
                for (var col = 0; col < virtualGrid.ColumnCount; col++)
                {
                    keyboardGrid[row, col] = ToColoreColor(virtualGrid[col, row]);
                }
            }

            return this.ChromaInterface!.Keyboard.SetCustomAsync(keyboardGrid);
        }
    }
}
