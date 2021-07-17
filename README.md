# VirtualGrid
[![nuget](https://img.shields.io/nuget/v/VirtualGrid.svg)](https://www.nuget.org/packages/VirtualGrid/)

A library to simplify RGB tasks when working with multiple devices and vendors by create single lightweight abstract LED array
and convert into device or vendor specific implementation via adapter at later state.

## How to use
First you need to create a `VirtualGrid.VirtualLedGrid` instance then create adapter instance to convert virtual LED grid to actual device effects.

```cs
// Create virtual grid instance with dimension of 30 columns x 9 rows.
IVirtualLedGrid virtualGrid = new VirtualGrid.VirtualLedGrid(30,9);

// Apply red color to whole grid, you can access each cell in grid using indexer.
virtualGrid.Set(VirtualGrid.Color.Red);

// Create adapter instance to convert virtual LED into actual device effect.
IPhysicalDeviceAdapter adapter = new RazerKeyboardAdapter();

// Apply virtual effect into actual effect.
await adapter.ApplyAsync(virtualGrid);
```

If you plan to create many adapters and would like to simplify effect managing task, there is a mediator to handle job for you.

```cs
// Create virtual grid instance with dimension of 30 columns x 9 rows.
IVirtualLedGrid virtualGrid = new VirtualGrid.VirtualLedGrid(30,9);

// Create mediator instance which will hold access to above virtual grid.
IDeviceArrangeMediator mediator = new VirtualGrid.PhysicalDeviceMediator(virtualGrid);

// Attach razer keyboard adapter at column 0 and row 0 of virtual grid.
mediator.Attach<RazerKeyboardAdapter>(0,0);
// Attach razer keyboard adapter at column 23 and row 0 of virtual grid.
mediator.Attach<RazerMouseAdapter>(23,0);
// Attach as many adapters as you want.

// Apply red color to whole grid, you can access each cell in grid using indexer.
virtualGrid.Set(VirtualGrid.Color.Red);

// Apply virtual effect into all connected adapters.
await mediator.ApplyAsync();
```

From version 2.1 onward you can create multiple virtual LED grid and combine them into a single grid like this.

```cs
IVirtualLedGrid layer1 = new VirtualLedGrid(30, 9);
IVirtualLedGrid layer2 = new VirtualLedGrid(30, 9);
IVirtualLedGrid layer3 = new VirtualLedGrid(15, 5);

layer1.Set(VirtualGrid.Color.Red);
layer2[0,0] = Color.Blue;
layer3[0,1] = Color.White;

// Merge them manually using + operator.
layer1 += layer2;
layer1 += layer3;

// Or attach them to mediator just like single grid and let the mediator handle the job instead.
IDeviceArrangeMediator mediator = new VirtualGrid.PhysicalDeviceMediator(layer1, layer2, layer3);

//Then apply the grid to adapter whatever way you want, manually or mediator will work just fine.
```