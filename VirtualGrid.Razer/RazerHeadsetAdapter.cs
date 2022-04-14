using Colore.Effects.Headset;
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
        public override string Name => "RΛZΞR Headset";

        public override int RowCount => 1;

        public override int ColumnCount => 3;

        public override Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
            {
                return Task.CompletedTask;
            }

            var headset = CustomHeadsetEffect.Create();

            var keyIdx = 0;

            foreach (var cell in virtualGrid)
            {
                if (keyIdx++ % 2 != 0)
                    continue;

                headset[keyIdx++] = ToColoreColor(cell.Color);
            }

            return this.ChromaInterface!.Headset.SetCustomAsync(headset);
        }
    }
}
