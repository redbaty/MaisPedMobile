using System;
using System.Linq;
using Mpm.ClientInjections;
using Ninject;
using Serilog;
using Serilog.Events;
using SGE32Helper;
using SGE32Helper.Helpers;

namespace MaisPedMobile.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level}][{SourceContext}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            var lastOrDefault = Sge32Helper.GetSges().LastOrDefault();
            var firstOrDefault = lastOrDefault.GetAlias().EnterpriseWrapper.FirstOrDefault();
            var kernel = new StandardKernel();
            kernel.InjectCommonClientComponents();

            var x = new SignalRSqlCrawler(new Addresses("", "http://localhost:6580/"),
                firstOrDefault?.CreateConnection(), kernel.Get<Authentication>());

            Console.ReadLine();
        }
    }
}