using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace StorageBlobDemo
{
    public class BlobStorageHelper
    {
        private const string STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=sonustorageaccount;AccountKey=pYRwypXNTAm+YW6k1lr/56f5hI62I9/leoLKKAhXQzkhERakZF0OQIJQOK5coUiqg05v4TdhjJLXkZiWugLTmQ==;EndpointSuffix=core.windows.net";
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient blobContainerClient;

        public BlobStorageHelper()
        {
            this.blobServiceClient = new BlobServiceClient(STORAGE_CONNECTION_STRING);
        }

        public async Task<BlobContainerClient> GetContainerAsync(string containerName)
        {
            blobContainerClient= blobServiceClient.GetBlobContainerClient(containerName);
            if(await blobContainerClient.ExistsAsync())
            {
                return blobContainerClient;
            }
            else
            {
                IDictionary<string, string> metadata = new Dictionary<string, string>()
                {
                    { "Author", "Sonu Sathyadas" }
                };
                await blobContainerClient.CreateAsync(PublicAccessType.BlobContainer, metadata);
                return blobContainerClient;
            }
        }

        public async Task<string> UploadFileAsync(string filePath, BlobContainerClient containerClient)
        {
            string blobName = Path.GetFileName(filePath);
            var blobClient=containerClient.GetBlobClient(blobName);
            if(!await blobClient.ExistsAsync())
            {
                FileStream fileStream = File.OpenRead(filePath);
                await blobClient.UploadAsync(fileStream);
                fileStream.Close();
                return blobClient.Uri.AbsoluteUri;
            }
            else
            {
                throw  new Exception("Blob already exists");
            }
        }

        public AsyncPageable<BlobItem> ListBlobs(string containerName)
        {
            blobServiceClient = new BlobServiceClient(STORAGE_CONNECTION_STRING);
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if(blobContainerClient.Exists())
            {
                return blobContainerClient.GetBlobsAsync();
            }
            else
            {
                throw new Exception("Container does not exists");
            }
        }
    }
}
