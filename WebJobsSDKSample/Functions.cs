using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebJobsSDKSample
{
    public class Functions
    {
        static HttpClient client = new HttpClient();
        static public void GetProductAsync(ILogger logger)
        {
            string path = "https://productsapp20210818221653.azurewebsites.net/api/Products";

            var response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string message=DateTime.Now.ToString()+' '+response.Content;
                logger.LogInformation(message);
                // await response.Content.ReadAsAsync();
            }
            return;
        }

        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, ILogger logger)
        {
            logger.LogInformation(message);
        }

    }
}
