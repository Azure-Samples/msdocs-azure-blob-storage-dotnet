using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Services
{
    public class BlobStorageService : IBlobStorageService
    {

        private BlobServiceClient _blobServiceClient;

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }


        public string GetStorageAccountName()
        {
            return _blobServiceClient.AccountName;
        }

        public async Task CreateContainerAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (await containerClient.ExistsAsync())
                throw new ApplicationException($"Unable to create container '{containerName}' as it already exists");

            await containerClient.CreateAsync();
        }


        public async IAsyncEnumerable<StorageContainerModel> GetContainersAsync()
        {
            AsyncPageable<BlobContainerItem> containers = _blobServiceClient.GetBlobContainersAsync();

            await foreach (var item in containers)
            {
                yield return new StorageContainerModel() { Name = item.Name };
            }
        }


        public async Task DeleteContainerAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!await containerClient.ExistsAsync())
                throw new ApplicationException($"Unable to delete container '{containerName}' as it does not exists");

            await containerClient.DeleteAsync();
        }

        public async IAsyncEnumerable<BlobInfoModel> ListBlobsInContainerAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!await containerClient.ExistsAsync())
                throw new ApplicationException($"Cannot list blobs in container '{containerName}' as it does not exists");

            AsyncPageable<BlobItem> blobs = containerClient.GetBlobsAsync();

            // Map the SDK objects to model objects and return them
            await foreach(var blob in blobs)
            {
                BlobInfoModel model = new BlobInfoModel()
                {
                    Name = blob.Name,
                    Tags = blob.Tags,
                    ContentEncoding = blob.Properties.ContentEncoding,
                    ContentType = blob.Properties.ContentType,
                    Size = blob.Properties.ContentLength,
                    CreatedOn = blob.Properties.CreatedOn,
                    AccessTier = blob.Properties.AccessTier?.ToString(),
                    BlobType = blob.Properties.BlobType?.ToString()
                };

                yield return model;
            }

        }


        public async Task UploadBlobAsync(string containerName, string blobName, string contentType, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!await containerClient.ExistsAsync())
                throw new ApplicationException($"Unable to upload blobs to container '{containerName}' as the container does not exists");

            var blobClient = containerClient.GetBlobClient(blobName);
            var options = new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType } };
            var response = await blobClient.UploadAsync(content, options);
        }


        public async Task<BlobModel> GetBlobContentsAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!await containerClient.ExistsAsync ())
                throw new ApplicationException($"Unable to get blob {blobName} in container '{containerName}' as the container does not exists");

            var blobClient = containerClient.GetBlobClient(blobName);
            if (!await blobClient.ExistsAsync())
                throw new ApplicationException($"Unable to delete blob {blobName} in container '{containerName}' as no blob with this name exists in this container");
                       
            return new BlobModel()
            {
                Name = blobName,
                ContentType = blobClient.GetProperties().Value.ContentType,
                Content = await blobClient.OpenReadAsync()
            };
        }


        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!await containerClient.ExistsAsync())
                throw new ApplicationException($"Unable to delete blob {blobName} in container '{containerName}' as the container does not exists");

            var blobClient = containerClient.GetBlobClient(blobName);
            if (!await blobClient.ExistsAsync() )
                throw new ApplicationException($"Unable to delete blob {blobName} in container '{containerName}' as no blob with this name exists in this container");

            await blobClient.DeleteAsync();
        }

    }
}
