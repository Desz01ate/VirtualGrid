using NvAPIWrapper.GPU;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using VirtualGrid;
using VirtualGrid.Razer;

namespace SystemMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new NotSupportedException("This application is designed to support only Windows OS.");
            }

            // This program is use to demonstrate the usage of VirtualGrid library
            // by capture some parameters from system information
            // and create an effect to reflect those parameters accordingly.

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            var totalMemoryMBytes = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1024 / 1024;
            var gpu = PhysicalGPU.GetPhysicalGPUs().FirstOrDefault();


            var grid = new VirtualLedGrid(30, 9);
            grid.Set(new Color(6, 6, 6));
            using var mediator = new PhysicalDeviceMediator(grid);
            mediator.Attach<RazerKeyboardAdapter>(0, 1);
            mediator.Attach<RazerMousepadAdapter>(0, 0);
            mediator.Attach<RazerMouseAdapter>(23, 0);
            mediator.Attach<RazerHeadsetAdapter>(25, 7);
            mediator.Attach<RazerChromaLinkAdapter>(12, 8);

            var cpuGrid = grid.Slice(2, 2, 12, 1);
            var memoryGrid = grid.Slice(2, 3, 12, 1);
            var diskGrid = grid.Slice(2, 4, 11, 1);
            var gpuGrid = grid.Slice(2, 5, 11, 1);


            var cpuColor = new Color(17, 125, 187);
            var memoryColor = new Color(139, 18, 174);
            var diskColor = new Color(77, 166, 12);

            var cpuIdleColor = new Color(2, 12, 19);
            var memoryAvailableColor = new Color(14, 2, 17);
            var diskAvailableColor = new Color(8, 17, 1);

            for (; ; )
            {
                cpuGrid.Set(cpuIdleColor);
                memoryGrid.Set(memoryAvailableColor);
                diskGrid.Set(diskAvailableColor);
                gpuGrid.Set(cpuIdleColor);

                var cpuUtilize = cpuCounter.NextValue() / 100f;
                var currentMemoryUsage = memoryCounter.NextValue();
                var memoryUtilize = 1.0f - (currentMemoryUsage / totalMemoryMBytes);
                var gpuUtilize = (gpu?.UsageInformation.GPU.Percentage ?? 0) / 100f;

                var cpuGridLength = (int)(cpuGrid.ColumnCount * cpuUtilize);
                var memoryGridLength = (int)(memoryGrid.ColumnCount * memoryUtilize);
                var gpuGridLegth = (int)(gpuGrid.ColumnCount * gpuUtilize);

                var diskInfo = new DriveInfo("C");
                var freeSpacePercent = (double)(diskInfo.TotalSize - diskInfo.TotalFreeSpace) / diskInfo.TotalSize;
                var diskGridLength = (int)(diskGrid.ColumnCount * (freeSpacePercent));

                for (var cpuCol = 0; cpuCol < cpuGridLength; cpuCol++)
                {
                    cpuGrid[cpuCol, 0] = cpuColor;
                }

                for (var memoryCol = 0; memoryCol < memoryGridLength; memoryCol++)
                {
                    memoryGrid[memoryCol, 0] = memoryColor;
                }

                for (var diskCol = 0; diskCol < diskGridLength; diskCol++)
                {
                    diskGrid[diskCol, 0] = diskColor;
                }
                for (var gpuCol = 0; gpuCol < gpuGridLegth; gpuCol++)
                {
                    gpuGrid[gpuCol, 0] = cpuColor;
                }

                await mediator.ApplyAsync();
                await Task.Delay(100);
            }
        }
    }
}
