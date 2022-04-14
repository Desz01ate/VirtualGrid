using Colore;

namespace VirtualGrid.Razer
{
    internal static class ChromaConnectorInterfaceSingleton
    {
        private static IChroma? _chromaConnector;
        private static readonly object _lock = new();

        public static IChroma ChromaConnector
        {
            get
            {
                lock (_lock)
                {
                    return _chromaConnector ??= ColoreProvider.CreateNativeAsync().Result;
                }
            }
        }
    }
}
