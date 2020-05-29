using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsDemo
{
    public static class Function1
    {
        [FunctionName("BlobTriggerFn")]
        public static void Run([BlobTrigger("input/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, 
            string name, ILogger log,
            [Blob("output/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")]Stream outBlob)
        {
            myBlob.CopyTo(outBlob);
            log.LogInformation("File copied");
        }
    }
}
