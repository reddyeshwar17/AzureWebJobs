using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace QueueTriggerJobs
{
    public class Functions
    {
        //public static void ProcessQueueMessage([QueueTrigger("stringqueue")] string message, ILogger logger)
        //{
        //    logger.LogInformation(message);
        //}

        //public static void ProcessQueueMessage(
        //    [QueueTrigger("stringqueue")] string message,
        //    [Blob("webjobscontainer/{queueTrigger}", FileAccess.Read)] Stream myBlob,
        //    ILogger logger)
        //{
        //    logger.LogInformation($"Blob name:{message} \n Size: {myBlob.Length} bytes");
        //}

        public static void ProcessQueueMessage(
            [QueueTrigger("stringqueue")] string message,
            [Blob("webjobscontainer/{queueTrigger}", FileAccess.Read)] Stream myBlob,
            [Blob("webjobscontainer/copy-{queueTrigger}", FileAccess.Write)] Stream outputBlob,
            ILogger logger)
        {
            logger.LogInformation($"Blob name:{message} \n Size: {myBlob.Length} bytes");
            myBlob.CopyTo(outputBlob);
        }
    }
}
