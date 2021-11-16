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

        public async Task CreateContainerAsync(string containerName)
        {
            await Task.CompletedTask;
        }


        public IAsyncEnumerable<StorageContainerModel> GetContainersAsync()
        {
            return new List<StorageContainerModel>().ToAsyncEnumerable();
        }


        public async Task DeleteContainerAsync(string containerName)
        {

        }

        public IAsyncEnumerable<BlobInfoModel> ListBlobsInContainerAsync(string containerName)
        {
            return new List<BlobInfoModel>().ToAsyncEnumerable();
        }


        public async Task UploadBlobAsync(string containerName, string blobName, string contentType, Stream content)
        {
            await Task.CompletedTask;
        }


        public async Task<BlobModel> GetBlobContentsAsync(string containerName, string blobName)
        {
            var model = new BlobModel()
            {
                Name = String.Empty,
                ContentType = String.Empty,
                Content = new MemoryStream()
            };
            return await Task.FromResult<BlobModel>(model);
        }


        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            await Task.CompletedTask;
        }

    }
}
