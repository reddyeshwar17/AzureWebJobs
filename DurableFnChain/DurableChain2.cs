using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DurableFnChain
{
    public class DurableChain2
    {
        [FunctionName("HelloArchestrator_HttpStart1")]
        public static async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("HelloArchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("HelloArchestrator")]
        public static async Task<List<string>> RunOrchestrator(
           [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("HelloArchestrator_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("HelloArchestrator_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("HelloArchestrator_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("HelloArchestrator_Hello")]
        public static string SayHello1([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }
    }
}
