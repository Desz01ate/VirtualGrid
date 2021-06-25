using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualGrid.Interfaces
{
    /// <summary>
    /// A mediator to arrange task between virtual grid and physical devices.
    /// </summary>
    public interface IArrangeMediator : IDisposable
    {
        /// <summary>
        /// Attach physical device adapter to mediator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">X-axis index to bind adapter to virtual grid.</param>
        /// <param name="y">Y-axis index to bind adapter to virtual grid.</param>
        void Attach<T>(int x, int y) where T : IPhysicalDeviceAdapter, new();

        /// <summary>
        /// Attach physical device adapter to mediator.
        /// </summary>
        /// <param name="x">X-axis index to bind adapter to virtual grid.</param>
        /// <param name="y">Y-axis index to bind adapter to virtual grid.</param>
        /// <param name="adapter">An instance of IPhysicalDeviceAdapter.</param>
        void Attach(int x, int y, IPhysicalDeviceAdapter adapter);

        /// <summary>
        /// Move adapter to different position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool UpdateAdapterPosition<T>(int x, int y) where T : IPhysicalDeviceAdapter;

        /// <summary>
        /// Detach physical device adapter from the mediator, the adapter will no longer control by mediator and must be manually manage by caller.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Instance of IPhysicalDeviceAdapter if type is registered in mediator, otherwise null.</returns>
        IPhysicalDeviceAdapter? Detach<T>() where T : IPhysicalDeviceAdapter;

        /// <summary>
        /// Apply rendered grid to physical devices.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ApplyAsync(CancellationToken cancellationToken = default);
    }
}
