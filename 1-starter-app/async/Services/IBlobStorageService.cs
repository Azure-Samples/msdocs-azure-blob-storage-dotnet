using AzureBlobStorageDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Services
{
    public interface IBlobStorageService
    {

        public IAsyncEnumerable<StorageContainerModel> GetContainersAsync();

        public Task CreateContainerAsync(string containerName);

        public Task DeleteContainerAsync(string containerName);

        public IAsyncEnumerable<BlobInfoModel> ListBlobsInContainerAsync(string containerName);

        public Task UploadBlobAsync(string containerName, string blobName, String contentType, Stream content);

        public Task DeleteBlobAsync(string containerName, string blobName);

        public Task<BlobModel> GetBlobContentsAsync(String containerName, string blobName);

    }
}
