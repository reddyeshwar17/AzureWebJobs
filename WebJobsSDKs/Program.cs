using Azure.Storage.Queues;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Configuration;

namespace WebJobsSDKs
{
    class Program
    {
        static void Main(string[] args)
        {
            /* string webJobsStorage = ConfigurationManager.AppSettings["AzureWebJobsStorage"];
             CloudStorageAccount storageAccount = CloudStorageAccount.Parse(webJobsStorage);
             CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
             CloudQueue strQueue = queueClient.GetQueueReference("webjobsqueue");
             strQueue.CreateIfNotExistsAsync();
             string msg = "Hello From the Client Application";
             strQueue.AddMessageAsync(new CloudQueueMessage(msg));
             //queueClient.Sen
             CloudQueue empQueue = queueClient.GetQueueReference("employeequeue"); 
             empQueue.CreateIfNotExistsAsync(); 
             Employee emp = new Employee() { Id = 1, Name = "E1", Salary = 10000 };
             empQueue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(emp)));*/

            string webJobsStorage = ConfigurationManager.AppSettings["AzureWebJobsStorage"];
            QueueClient queueClient = new QueueClient(webJobsStorage, "stringqueue");
            queueClient.CreateIfNotExists();
            string msg = "Hello From the Client Application";
            queueClient.SendMessage(msg);

            QueueClient qClient = new QueueClient(webJobsStorage, "employeequeue");
            qClient.CreateIfNotExists();            
            Employee emp = new Employee() { Id = 1, Name = "E1", Salary = 10000 };
            qClient.SendMessage(JsonConvert.SerializeObject(emp));
        }


        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Salary { get; set; }
        }
    }
}
