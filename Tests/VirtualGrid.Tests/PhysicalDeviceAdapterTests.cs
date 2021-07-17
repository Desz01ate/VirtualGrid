using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Interfaces;
using VirtualGrid.Tests.Adapters;
using Xunit;

namespace VirtualGrid.Tests
{
    public class PhysicalDeviceAdapterTests
    {
        [Fact]
        public void PhysicalDeviceAdapter_GetName_ShouldReturnCorrectName()
        {
            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(a => a.Name).Returns("TestAdapter");

            var expected = "TestAdapter";
            var actual = mockAdapter.Object.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task PhysicalDeviceAdapter_ApplyAsync_WithVirtualGrid_ShouldSuccess()
        {
            var mockGrid = new Mock<IVirtualLedGrid>();

            var mockAdapter = new Mock<IPhysicalDeviceAdapter>();
            mockAdapter.Setup(a => a.ApplyAsync(It.IsAny<IVirtualLedGrid>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            await mockAdapter.Object.ApplyAsync(mockGrid.Object);
        }
    }
}
