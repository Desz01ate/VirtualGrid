using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;
using VirtualGrid.Tests.Adapters;
using Xunit;

namespace VirtualGrid.Tests
{
    public class PhysicalDeviceMediatorTests
    {
        [Fact]
        public void CreateMediator_WithIVirtualLedGrid_ShouldSuccess()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);

            var expectedGridCount = 1;
            var actualGridCount = mediator.AttachedLedGrids.Count();

            Assert.Equal(expectedGridCount, actualGridCount);
        }

        [Fact]
        public void CreateMediator_WithNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PhysicalDeviceMediator((IVirtualLedGrid)null));
        }

        [Fact]
        public void CreateMediator_WithManyIVirtualLedGrid_ShouldSuccess()
        {
            var mockGrid1 = new Mock<IVirtualLedGrid>();
            var mockGrid2 = new Mock<IVirtualLedGrid>();

            var mediator = new PhysicalDeviceMediator(mockGrid1.Object, mockGrid2.Object);

            var expectedGridCount = 2;
            var actualGridCount = mediator.AttachedLedGrids.Count();

            Assert.Equal(expectedGridCount, actualGridCount);
        }

        [Fact]
        public void CreateMediator_WithIVirtualLedGrid_AndNull_ShouldThrowsArgumentException()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();

            Assert.Throws<ArgumentException>(() => new PhysicalDeviceMediator(mockGrid.Object, null));
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 0)]
        [InlineData(0, 0)]
        public void AttachAdapter_ToMediator_ShouldSuccess(int attachX, int attachY)
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(x => x.Initialized).Returns(true);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            mediator.Attach(attachX, attachY, mockAdapter.Object);

            var expectedAdapterCount = 1;
            var actualAdapterCount = mediator.AttachedAdapters.Count();

            Assert.Equal(expectedAdapterCount, actualAdapterCount);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        public void AttachAdapter_ToMediator_WithNegativeIndex_ShouldThrowArgumentOutOfRangeException(int attachX, int attachY)
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(x => x.Initialized).Returns(true);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            Assert.Throws<ArgumentOutOfRangeException>(() => mediator.Attach(attachX, attachY, mockAdapter.Object));
        }

        [Fact]
        public void AttachAdapter_ToMediator_WithNullAdapter_ShouldThrowArgumentNullException()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            Assert.Throws<ArgumentNullException>(() => mediator.Attach(0, 0, null));
        }

        [Fact]
        public void AttachAdapter_ToMediator_WithUnInitializeAdapter_ShouldSkipAttach()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(x => x.Initialized).Returns(false);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            mediator.Attach(0, 0, mockAdapter.Object);

            var expectedAdapterCount = 0;
            var actualAdapterCount = mediator.AttachedAdapters.Count();

            Assert.Equal(expectedAdapterCount, actualAdapterCount);
        }

        [Fact]
        public void AttachAdapter_ToMediator_WithExistingAdapter_ShouldThrowInvalidOperationException()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(x => x.Initialized).Returns(true);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            mediator.Attach(0, 0, mockAdapter.Object);
            Assert.Throws<InvalidOperationException>(() => mediator.Attach(0, 0, mockAdapter.Object));
        }

        [Fact]
        public void UpdateAdapterPosition_WithExistingAdapterType_ShouldReturnTrue()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            mediator.Attach<TestAdapter>(0, 0);
            var actualUpdateStatus = mediator.UpdateAdapterPosition<TestAdapter>(1, 1);

            Assert.True(actualUpdateStatus);
        }

        [Fact]
        public void UpdateAdapterPosition_WithNotExistAdapterType_ShouldReturnFalse()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            var actualUpdateStatus = mediator.UpdateAdapterPosition<TestAdapter>(1, 1);

            Assert.False(actualUpdateStatus);
        }

        [Fact]
        public void DetachExistingAdapter_FromMediator_ShouldReturnAdapterInstance()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var expectedAdapter = new TestAdapter();

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            mediator.Attach(0, 0, expectedAdapter);

            var actualDetachAdapter = mediator.Detach<TestAdapter>();

            Assert.Same(expectedAdapter, actualDetachAdapter);
        }

        [Fact]
        public void DetachNotExistAdapter_FromMediator_ShouldReturnNull()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();
            mockGrid.Setup(x => x.ColumnCount).Returns(30);
            mockGrid.Setup(x => x.RowCount).Returns(9);

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(x => x.Initialized).Returns(true);

            var mediator = new PhysicalDeviceMediator(mockGrid.Object);
            mediator.Attach(0, 0, mockAdapter.Object);

            var actualDetachAdapter = mediator.Detach<TestAdapter>();

            Assert.Null(actualDetachAdapter);
        }

        [Fact]
        public async Task Apply_LedGrid_ToAdapter_ShouldSuccess()
        {
            var grid = new VirtualLedGrid(30, 9);

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(x => x.ColumnCount).Returns(20);
            mockAdapter.Setup(x => x.RowCount).Returns(9);
            mockAdapter.Setup(x => x.Initialized).Returns(true);

            var mediator = new PhysicalDeviceMediator(grid);
            mediator.Attach(0, 0, mockAdapter.Object);

            grid.Set(Color.Red);

            await mediator.ApplyAsync();
        }
    }
}
