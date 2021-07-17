using System;
using Xunit;

namespace VirtualGrid.Tests
{
    public class VirtualLedGridTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 10)]
        [InlineData(10, 0)]
        [InlineData(10, 10)]
        public void CreateVirtualLedGrid_WithPositiveIndex_ShouldSuccess(int column, int row)
        {
            var instance = new VirtualLedGrid(column, row);
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void CreateVirtualLedGrid_WithNegativeIndex_ShouldThrowArgumentOutOfRange(int column, int row)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VirtualLedGrid(column, row));
        }

        [Fact]
        public void SetColorToVirtualLedGrid_AllKeyShouldChangeAccordingly()
        {
            var givenColor = Color.Green;
            var expectedColor = Color.Green;
            var instance = new VirtualLedGrid(2, 2);

            instance.Set(givenColor);
            Assert.All(instance, (x) => Assert.Equal(expectedColor, x.Color.Value));
        }

        [Fact]
        public void SetNullColorToVirtualLedGrid_AllKeyShouldChangeAccordingly()
        {
            Color? givenColor = null;
            Color? expectedColor = null;
            var instance = new VirtualLedGrid(2, 2);

            instance.Set(givenColor);

            Assert.All(instance, (x) => Assert.Equal(expectedColor, x.Color));
        }

        [Fact]
        public void SetColorsToVirtualLedGrid_ShouldSetKeyColorCorrectly()
        {
            var givenColors = new Color?[2][]
            {
                new Color?[] { Color.Red, Color.Green },
                new Color?[] { Color.Blue, Color.White }
            };
            var expectedColors = new Color?[2][]
            {
                new Color?[] { Color.Red, Color.Green },
                new Color?[] { Color.Blue, Color.White }
            };
            var instance = new VirtualLedGrid(2, 2);

            instance.Set(givenColors);

            foreach (var key in instance)
            {
                var col = key.Index.X;
                var row = key.Index.Y;
                var expectedColor = expectedColors[row][col];
                Assert.Equal(expectedColor, key.Color);
            }
        }

        [Fact]
        public void ClearColorFromVirtualLedGrid_ShouldSetAllKeyColorToNull()
        {
            var instance = new VirtualLedGrid(2, 2);

            instance.Set(Color.Green);
            instance.Clear();
            Assert.All(instance, (x) => Assert.Null(x.Color));
        }

        [Theory]
        [InlineData(10, 10, 0, 0, 2, 2)]
        [InlineData(10, 10, 5, 5, 2, 2)]
        public void SliceGridFromVirtualLedGrid_ShouldSliceCorrectly(int gridCol, int gridRow, int column, int row, int columnCount, int rowCount)
        {
            var instance = new VirtualLedGrid(gridCol, gridRow);

            var subGrid = instance.Slice(column, row, columnCount, rowCount);

            var expectedColumnCount = columnCount;
            var actualColumnCount = subGrid.ColumnCount;

            var expectedRowCount = rowCount;
            var actualRowCount = subGrid.RowCount;

            Assert.Equal(expectedColumnCount, actualColumnCount);
            Assert.Equal(expectedRowCount, actualRowCount);
        }

        [Theory]
        [InlineData(10, 10, 9, 9, 2, 2)]
        [InlineData(50, 50, 25, 45, 100, 100)]
        public void SliceGridFromVirtualLedGrid_WithOverflowIndex_ShouldSliceCorrectly(int gridCol, int gridRow, int column, int row, int columnCount, int rowCount)
        {
            var instance = new VirtualLedGrid(gridCol, gridRow);

            var subGrid = instance.Slice(column, row, columnCount, rowCount);

            var expectedColumnCount = gridCol - column;
            var actualColumnCount = subGrid.ColumnCount;

            var expectedRowCount = gridRow - row;
            var actualRowCount = subGrid.RowCount;

            Assert.Equal(expectedColumnCount, actualColumnCount);
            Assert.Equal(expectedRowCount, actualRowCount);
        }

        [Fact]
        public void PlusOperator_Operate_BetweenObjects_ShouldSuccess()
        {
            var instance = new VirtualLedGrid(1, 1);
            var instance2 = new VirtualLedGrid(1, 1);

            var actual = instance + instance2;

            Assert.NotNull(actual);
        }

        [Fact]
        public void PlusOperator_Operate_OnValidAndNullObject_ShouldThrowInvalidOperationException()
        {
            VirtualLedGrid instance = new VirtualLedGrid(1, 1);
            VirtualLedGrid instance2 = null;

            Assert.Throws<InvalidOperationException>(() => instance + instance2);
        }

        [Fact]
        public void PlusOperator_Operate_OnNullObjects_ShouldThrowInvalidOperationException()
        {
            VirtualLedGrid instance = null;
            VirtualLedGrid instance2 = null;

            Assert.Throws<InvalidOperationException>(() => instance + instance2);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        public void Indexer_WithProperIndex_ShouldReturnColor(int x, int y)
        {
            VirtualLedGrid instance = new VirtualLedGrid(2, 2);
            instance.Set(Color.Red);

            var expected = Color.Red;
            var actual = instance[x, y];

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void Indexer_WithInvalidIndex_ShouldThrowIndexOutOfRange(int x, int y)
        {
            VirtualLedGrid instance = new VirtualLedGrid(2, 2);
            instance.Set(Color.Red);

            Assert.Throws<IndexOutOfRangeException>(() => instance[x, y]);
        }
    }
}
