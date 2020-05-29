using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;

namespace StorageBlobDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync();
            Console.ReadLine();
        }

        private static async void RunAsync()
        {
            string containerName;
            BlobContainerClient containerClient;
            BlobStorageHelper storageHelper = new BlobStorageHelper();

            Console.WriteLine("1) Create container");
            Console.WriteLine("2) Upload file");
            Console.WriteLine("3) List blobs");            
            Console.Write("Enter your choice:");
            int choice = Int32.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter container name:");
                    containerName = Console.ReadLine();
                    await storageHelper.GetContainerAsync(containerName);
                    Console.WriteLine("Container created.");
                    break;
                case 2:
                    Console.WriteLine("Enter file path:");
                    string filePath = Console.ReadLine();
                    Console.WriteLine("Enter container name:");
                    containerName = Console.ReadLine();
                    containerClient = await storageHelper.GetContainerAsync(containerName);
                    string blobUri = await storageHelper.UploadFileAsync(filePath, containerClient);
                    Console.WriteLine($"File uploaded. Uri :{blobUri}");
                    break;

                case 3:
                    Console.WriteLine("Enter container name:");
                    containerName = Console.ReadLine();
                    containerClient = await storageHelper.GetContainerAsync(containerName);
                    var blobs = storageHelper.ListBlobs(containerName);
                    await foreach (BlobItem blobItem in blobs)
                    {
                        Console.WriteLine("Blob name:" + blobItem.Name);
                        Console.WriteLine("Blob Type:" + blobItem.Properties.BlobType);
                    }
                    break;                

            }
            
        }
    }
}
