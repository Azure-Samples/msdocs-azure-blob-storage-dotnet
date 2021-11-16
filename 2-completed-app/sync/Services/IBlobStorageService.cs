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

        public string GetStorageAccountName();


        public IEnumerable<StorageContainerModel> GetContainers();

        public void CreateContainer(string containerName);

        public void DeleteContainer(string containerName);

        public IEnumerable<BlobInfoModel> ListBlobsInContainer(string containerName);

        public void UploadBlob(string containerName, string blobName, String contentType, Stream content);

        public void DeleteBlob(string containerName, string blobName);

        public BlobModel GetBlobContents(String containerName, string blobName);

    }
}
