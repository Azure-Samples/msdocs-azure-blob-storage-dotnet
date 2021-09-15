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

        public void CreateContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (containerClient.Exists())
                throw new ApplicationException($"Unable to create container '{containerName}' as it already exists");

            containerClient.Create();
        }


        public IEnumerable<StorageContainerModel> GetContainers()
        {
            Pageable<BlobContainerItem> response = _blobServiceClient.GetBlobContainers();

            return response.Select(c => new StorageContainerModel() { Name = c.Name });
        }


        public void DeleteContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!containerClient.Exists())
                throw new ApplicationException($"Unable to delete container '{containerName}' as it does not exists");

            containerClient.Delete();
        }

        public IEnumerable<BlobInfoModel> ListBlobsInContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!containerClient.Exists())
                throw new ApplicationException($"Cannot list blobs in container '{containerName}' as it does not exists");

            Pageable<BlobItem> blobs = containerClient.GetBlobs();
            var models = blobs.Select(b => new BlobInfoModel()
            {
                Name = b.Name,
                Tags = b.Tags,
                ContentEncoding = b.Properties.ContentEncoding,
                ContentType = b.Properties.ContentType,
                Size = b.Properties.ContentLength,
                CreatedOn = b.Properties.CreatedOn,
                AccessTier = b.Properties.AccessTier?.ToString(),
                BlobType = b.Properties.BlobType?.ToString()
            });

            return models;
        }


        public void UploadBlob(string containerName, string blobName, string contentType, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
                throw new ApplicationException($"Unable to upload blobs to container '{containerName}' as the container does not exists");

            var blobClient = containerClient.GetBlobClient(blobName);
            var options = new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType } };
            var response = blobClient.Upload(content, options);
        }


        public BlobModel GetBlobContents(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
                throw new ApplicationException($"Unable to get blob {blobName} in container '{containerName}' as the container does not exists");

            var blobClient = containerClient.GetBlobClient(blobName);
            if (!blobClient.Exists())
                throw new ApplicationException($"Unable to delete blob {blobName} in container '{containerName}' as no blob with this name exists in this container");
                       
            return new BlobModel()
            {
                Name = blobName,
                ContentType = blobClient.GetProperties().Value.ContentType,
                Content = blobClient.OpenRead()
            };
        }


        public void DeleteBlob(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
                throw new ApplicationException($"Unable to delete blob {blobName} in container '{containerName}' as the container does not exists");

            var blobClient = containerClient.GetBlobClient(blobName);
            if (!blobClient.Exists() )
                throw new ApplicationException($"Unable to delete blob {blobName} in container '{containerName}' as no blob with this name exists in this container");

            blobClient.Delete();
        }

    }
}
