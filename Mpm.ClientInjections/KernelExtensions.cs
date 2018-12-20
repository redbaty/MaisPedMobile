using System.IO;
using System.Reflection;
using Ninject;

namespace Mpm.ClientInjections
{
    public static class KernelExtensions
    {
        public static void InjectCommonClientComponents(this IKernel kernel)
        {
            kernel.Bind<Addresses>()
                .ToConstant(new Addresses(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    "http://localhost:6580/"));
            kernel.Bind<Authentication>().ToSelf().InSingletonScope();
        }
    }
}