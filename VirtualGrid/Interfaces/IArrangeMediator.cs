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
        void Attach<T>() where T : IPhysicalDeviceAdapter, new();
        /// <summary>
        /// Attach physical device adapter to mediator.
        /// </summary>
        /// <param name="adapter">An instance of IPhysicalDeviceAdapter.</param>
        void Attach(IPhysicalDeviceAdapter adapter);
        /// <summary>
        /// Apply rendered grid to physical devices.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ApplyAsync(CancellationToken cancellationToken = default);
    }
}
