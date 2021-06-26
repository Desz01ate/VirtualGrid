namespace VirtualGrid.Interfaces
{
    /// <summary>
    /// Virtual key that represent a physical key in virtual grid.
    /// </summary>
    public interface IVirtualKey

    {
        /// <summary>
        /// Index of key in virtual grid.
        /// </summary>
        (int X, int Y) Index { get; }

        /// <summary>
        /// Color of key.
        /// </summary>
        Color? Color { get; set; }
    }
}
