# VirtualGrid
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