using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace WebJobsSDKSample
{
    public class LogEntity : TableEntity
    {

        public string Message { get; set; }
        //public DateTime TimeStamp { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
    public class Functions
    {
        static HttpClient client = new HttpClient();
        static public void GetProductAsync()
        {
            string path = "https://productsapp20210818221653.azurewebsites.net/api/Products";
            client.BaseAddress = new Uri(path);
            client.DefaultRequestHeaders.Accept.Add(
           new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                var message = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                LogMessage(message);


                //logger.LogInformation(message);

            }
            return;
        }

        static public void PutProductAsync()
        {
            var product = new Product { Id = 1, Name = "Yo-yo", Category = "Toys", Price = 3.75M };

            string output = JsonConvert.SerializeObject(product);

            var requestContent = new StringContent(output, Encoding.UTF8, "application/json");

            string path = "https://productsapp20210818221653.azurewebsites.net/api/Products";
            client.BaseAddress = new Uri(path);
            client.DefaultRequestHeaders.Accept.Add(
           new MediaTypeWithQualityHeaderValue("application/json"));


            var response = client.PutAsync(path, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var message = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                LogMessage(message);


                //logger.LogInformation(message);

            }
            return;
        }
        private static CloudTable AuthTable()
        {
            string accountName = "mystorageaccountsummer";
            string accountKey = "e8J+fsSWFlxP8H5yny1oSMPC9VVKZeHyRQJkpkConkX2lzUUWRal0KWyhmitaelkKNtDfIvquy7DcTdMAP6hcA==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudTableClient client = account.CreateCloudTableClient();

                CloudTable table = client.GetTableReference("LogProduct");

                return table;
            }
            catch
            {
                return null;
            }
        }
        private static bool CreateEntity(string message, CloudTable table)
        {
            var newEntity = new LogEntity()
            {
                PartitionKey = DateTime.Now.ToString("yyyyMMdd"),
                RowKey = Guid.NewGuid().ToString(),
                //TimeStamp= DateTime.Now,
                Message = message
            };

            TableOperation insert = TableOperation.Insert(newEntity);
            try
            {

                table.Execute(insert);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void LogMessage(string message)
        {
            var table = AuthTable();



            var success = CreateEntity(message, table);



        }

        //public static void ProcessQueueMessage([QueueTrigger("queue")] string message, ILogger logger)
        //{
        //    logger.LogInformation(message);
        //}

    }
}
