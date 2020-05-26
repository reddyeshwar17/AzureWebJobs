using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;

namespace QueueTriggerJobs
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();
            //builder.UseEnvironment(EnvironmentName.Development);

            //https://docs.microsoft.com/en-us/azure/app-service/webjobs-sdk-how-to#azure-cosmosdb-trigger-configuration-version-3x
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                //b.AddAzureStorage();
                b.AddCosmosDB(a =>
                {
                    a.ConnectionMode = ConnectionMode.Gateway;
                    a.Protocol = Protocol.Https;
                    a.LeaseOptions.LeasePrefix = "prefix1";

                });


                b.AddServiceBus(sbOptions =>
                {
                    sbOptions.MessageHandlerOptions.AutoComplete = true;
                    sbOptions.MessageHandlerOptions.MaxConcurrentCalls = 16;
                });
            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
                // If the key exists in settings, use it to enable Application Insights.
                string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    b.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
                }
            });

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
