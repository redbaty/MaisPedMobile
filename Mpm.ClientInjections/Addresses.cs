using System;
using System.Linq;

namespace Mpm.ClientInjections
{
    public class Addresses
    {
        public Addresses(string basePath, string serverUrl)
        {
            BasePath = basePath;
            ServerUrl = serverUrl;
        }

        public string BasePath { get; }

        public string ServerUrl { get; }
    }
}