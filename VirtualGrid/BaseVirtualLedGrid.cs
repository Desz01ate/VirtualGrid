using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualGrid.Interfaces;

namespace VirtualGrid
{
    public abstract class BaseVirtualLedGrid : IVirtualLedGrid
    {
        public abstract Color? this[int column, int row] { get; set; }

        public abstract int RowCount { get; }

        public abstract int ColumnCount { get; }

        public abstract IEnumerator<IVirtualKey> GetEnumerator();

        public abstract void Set(Color? color);

        public abstract void Set(Color?[][] colors);

        public abstract void Clear();

        /// <summary>
        /// Take a slice grid within given criteria.
        /// </summary>
        /// <param name="column">Start column index.</param>
        /// <param name="row">Start row index.</param>
        /// <param name="columnCount">Total column to slice.</param>
        /// <param name="rowCount">Total row to slice.</param>
        /// <returns>A sliced virtual LED grid if criteria is in proper range, otherwise null.</returns>
        internal protected abstract IVirtualLedGrid? Slice(int column, int row, int columnCount, int rowCount);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static BaseVirtualLedGrid operator +(BaseVirtualLedGrid ledGrid, BaseVirtualLedGrid anotherVirtualGrid)
        {
            if (ledGrid == null && anotherVirtualGrid == null)
            {
                throw new InvalidOperationException();
            }

            var gridZip = ledGrid.Zip(anotherVirtualGrid, (l, r) => (Layer1: l, Layer2: r));
            foreach (var pair in gridZip)
            {
                var top = pair.Layer2;
                var bottom = pair.Layer1;

                if (top?.Color != null)
                {
                    bottom.Color = top.Color;
                }
            }
            return ledGrid;
        }
    }
}
