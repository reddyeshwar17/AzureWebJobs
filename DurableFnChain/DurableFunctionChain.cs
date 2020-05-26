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
    public class DurableFunctionChain
    {
        [FunctionName("HttpStart")]
        public static async Task<HttpResponseMessage> start(
           [HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = "orchestrators/{functionName}")] HttpRequestMessage req,
           [DurableClient] IDurableClient starter,
           string functionName,
           ILogger log)
        {
            // Function input comes from the request content.
            object eventData = await req.Content.ReadAsAsync<object>();
            string instanceId = await starter.StartNewAsync(functionName, eventData);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }


        [FunctionName("E1_HelloSequence")]
        public static async Task<List<string>> Run(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("E1_SayHello1", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("E1_SayHello1", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("E1_SayHello_DirectInput", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("E1_SayHello")]
        public static string SayHello1([ActivityTrigger] IDurableActivityContext context)
        {
            string name = context.GetInput<string>();
            return $"Hello {name}!";
        }

        [FunctionName("E1_SayHello_DirectInput")]
        public static string SayHelloDirectInput([ActivityTrigger] string name)
        {
            return $"Hello {name}!";
        }

        //-----------------------------------------------------
       
    }
}
