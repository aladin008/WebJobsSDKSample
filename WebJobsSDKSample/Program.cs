using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebJobsSDKSample
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello World!");
        //}
        static async Task Main()
        {
            var builder = new HostBuilder(); 
            
            builder.UseEnvironment(EnvironmentName.Development);

            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices(); 
                b.AddAzureStorage();

            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }


    }
}
