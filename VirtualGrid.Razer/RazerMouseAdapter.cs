using Colore.Effects.Mouse;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Razer
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Razer-compatible mouse effects.
    /// </summary>
    public sealed class RazerMouseAdapter : RazerPeripheralBaseAdapter
    {
        public override string Name => "RΛZΞR Mouse";

        public override int RowCount => 9;

        public override int ColumnCount => 7;

        public override async Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
                return;

            var mouseGrid = CustomMouseEffect.Create();

            for (var row = 0; row < virtualGrid.RowCount; row++)
            {
                for (var col = 0; col < virtualGrid.ColumnCount; col++)
                {
                    mouseGrid[row, col] = ToColoreColor(virtualGrid[col, row].Value);
                }
            }

            await this.ChromaInterface.Mouse.SetGridAsync(mouseGrid);
        }
    }
}
