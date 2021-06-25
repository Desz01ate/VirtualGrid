using System;
using System.Collections;
using System.Collections.Generic;
using VirtualGrid.Interfaces;

namespace VirtualGrid
{
    /// <summary>
    /// A virtual LED grid that represent the physical setup.
    /// </summary>
    public sealed class VirtualLedGrid : IVirtualLedGrid
    {
        class VirtualKey : IVirtualKey
        {
            public (int X, int Y) Index { get; }

            public Color Color { get; set; }

            public VirtualKey(int x, int y, Color color = default)
            {
                this.Index = (x, y);
                this.Color = color;
            }

            public override string ToString()
            {
                return $"({Index.X},{Index.Y}): {Color.Value}";
            }
        }

        private readonly int _totalRowCount;
        private readonly int _totalColumnCount;
        private readonly IVirtualKey[][] _grid;

        public int RowCount => _totalRowCount;

        public int ColumnCount => _totalColumnCount;

        /// <summary>
        /// Indexer for accesing color of the virtual grid using specific index.
        /// </summary>
        /// <param name="column">Specific column.</param>
        /// <param name="row">Specific row.</param>
        /// <returns><seealso cref="Color"/></returns>
        public Color this[int column, int row]
        {
            get
            {
                return _grid[row][column].Color;
            }

            set
            {
                _grid[row][column].Color = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public VirtualLedGrid(int column, int row)
        {
            var grid = new VirtualKey[row][];
            for (var rowCount = 0; rowCount < row; rowCount++)
            {
                var rowArray = new VirtualKey[column];
                for (var columnCount = 0; columnCount < column; columnCount++)
                {
                    rowArray[columnCount] = new VirtualKey(columnCount, rowCount);
                }
                grid[rowCount] = rowArray;
            }
            this._grid = grid;
            this._totalColumnCount = column;
            this._totalRowCount = row;
        }

        private VirtualLedGrid(IVirtualKey[][] grid, int column, int row)
        {
            this._grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this._totalColumnCount = column;
            this._totalRowCount = row;
        }

        /// <summary>
        /// Set a single color to all keys.
        /// </summary>
        /// <param name="color"></param>
        public void Set(Color color)
        {
            foreach (var row in this._grid)
                foreach (var key in row)
                    key.Color = color;
        }

        /// <summary>
        /// Set per-key color.
        /// </summary>
        /// <param name="colors"></param>
        public void Set(Color[][] colors)
        {
            if (colors == null)
                throw new ArgumentNullException(nameof(colors));

            for (var y = 0; y < colors.GetLength(0); y++)
            {
                var row = colors[y];
                for (var x = 0; x < row.Length; x++)
                {
                    this[x, y] = row[x];
                }
            }
        }

        public IEnumerator<IVirtualKey> GetEnumerator()
        {
            foreach (var row in this._grid)
                foreach (var key in row)
                    yield return key;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IVirtualLedGrid? Slice(int column, int row, int columnCount, int rowCount)
        {
            var requestedColumn = column + columnCount;
            var requestedRow = row + rowCount;
            if (requestedColumn > this._totalColumnCount)
            {
                requestedColumn = this._totalColumnCount - 1 - column;
            }

            if (requestedRow > this._totalRowCount)
            {
                requestedRow = this._totalRowCount - 1 - row;
            }

            //even after adjustment, still exceed grid dimension.
            if (requestedColumn < 0 || this._totalColumnCount < requestedColumn ||
                requestedRow < 0 || this._totalRowCount < requestedRow)
            {
                return null;
            }

            var grid = new IVirtualKey[rowCount][];
            var subRow = this._grid[row..requestedRow];
            for (var rowIdx = 0; rowIdx < subRow.Length; rowIdx++)
            {
                var currentRow = subRow[rowIdx][column..requestedColumn];
                grid[rowIdx] = currentRow;
            }
            return new VirtualLedGrid(grid, requestedColumn - column, requestedRow - row);
        }
    }
}
