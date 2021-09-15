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

        public void CreateContainer(string containerName)
        {

        }


        public IEnumerable<StorageContainerModel> GetContainers()
        {
            return new List<StorageContainerModel>();
        }


        public void DeleteContainer(string containerName)
        {

        }

        public IEnumerable<BlobInfoModel> ListBlobsInContainer(string containerName)
        {
            return new List<BlobInfoModel>();
        }


        public void UploadBlob(string containerName, string blobName, string contentType, Stream content)
        {

        }


        public BlobModel GetBlobContents(string containerName, string blobName)
        {

            return new BlobModel()
            {
                Name = String.Empty,
                ContentType = String.Empty,
                Content = new MemoryStream()
            };
        }


        public void DeleteBlob(string containerName, string blobName)
        {

        }

    }
}
