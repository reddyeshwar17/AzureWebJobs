using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;


namespace QueueTriggerJobs
{
    public static class WriteOneDoc
    {
        [FunctionName("WriteOneDoc")]
        public static void Run(
            [QueueTrigger("todoqueueforwrite")] string queueMessage,
            [CosmosDB(
                databaseName: "ToDoItems",
                collectionName: "Items",
                ConnectionStringSetting = "CosmosDBConnection")]out ToDoItem document,
            ILogger log)
        {
            document = new ToDoItem { Description = queueMessage, Id = Guid.NewGuid().ToString() };

            log.LogInformation($"C# Queue trigger function inserted one row");
            log.LogInformation($"Description={queueMessage}");
        }

        public class ToDoItem
        {
            public string Id { get; set; }
            public string Description { get; set; }
        }
    }
}
