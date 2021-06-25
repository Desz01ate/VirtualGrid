using Colore;
using System;
using System.Collections.Generic;
using System.Text;

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
