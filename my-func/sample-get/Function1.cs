﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web.Http;

namespace my_func.sampleget
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string queryParameter = req.Query["q"];
            if (string.IsNullOrEmpty(queryParameter))
                return new BadRequestResult();

            if (queryParameter == "ThisStringCausesTheFunctionToThrowAnError")
                return new InternalServerErrorResult();

            return new OkResult();
        }
    }

}

