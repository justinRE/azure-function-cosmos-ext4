using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CosmosDBSamplesV2;

namespace com.wesleyreisz.example
{
    public static class PostItem
    {
        [FunctionName("PostItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                 databaseName: "my-database",
                 containerName: "my-container",
                 Connection = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //this reads the body and deserializes it to json
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            //this is a shortcut to creating an object. You should create an object
            //structure in the src folder and then use that in your logic. I did this
            //for validation purposes.
            if (requestBody.Contains("name") && 
                requestBody.Contains("email") &&
                requestBody.Contains("phone"))
            {
                var item = new ToDoItem();
                // Add a JSON document to the output container.
                await documentsOut.AddAsync(new
                {
                    // create a random ID
                    id = System.Guid.NewGuid().ToString(),
                    customerName = data.name,
                    customerPhone = data.phone,
                    customerEmail = data.email
                });
            }

            string responseMessage = string.IsNullOrEmpty(data)
                ? "Please add make this request with name, phone, email"
                : $"This HTTP triggered function executed successfully and preserved in cosmosdb using {data.name},{data.phone},{data.email} .";

            return new OkObjectResult(responseMessage);
        }
    }
}
