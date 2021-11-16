using AzureBlobStorageDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Services
{
    public class TestBlobStorageService : IBlobStorageService
    {

        private List<StorageContainerModel> storageContainers = new List<StorageContainerModel>()
        {
            new StorageContainerModel() { Name = "photos" },
            new StorageContainerModel() { Name = "ebooks" },
            new StorageContainerModel() { Name = "videos" },
        };

        private Dictionary<string, List<BlobInfoModel>> blobsPerContainer = new Dictionary<string, List<BlobInfoModel>>()
        {
            { "photos", new List<BlobInfoModel>() 
                { 
                    new BlobInfoModel() {Name = "DotNetBotChillin", CreatedOn = DateTime.Now.AddHours(-1), ContentType = "image/png", Size = 29000},
                    new BlobInfoModel() {Name = "DotNetBotGrillin", CreatedOn = DateTime.Now.AddMinutes(-59), ContentType = "image/png", Size = 33050},
                    new BlobInfoModel() {Name = "AzureLogo", CreatedOn = DateTime.Now.AddHours(-3), ContentType = "image/svg", Size = 12000},
                }
            },
            { "ebooks", new List<BlobInfoModel>()
                {
                    new BlobInfoModel() {Name = "Getting Started with Azure", CreatedOn = DateTime.Now.AddHours(-67), ContentType = "application/pdf", Size = 4302000}
                }
            },
            { "videos", new List<BlobInfoModel>()
            }
        };


        public void CreateContainer(string containerName)
        {
            var container = new StorageContainerModel() { Name = containerName };
            storageContainers.Add(container);
            blobsPerContainer.Add(containerName, new List<BlobInfoModel>());
        }

        public void DeleteBlob(string containerName, string blobName)
        {
            if (!blobsPerContainer.ContainsKey(containerName))
                throw new ApplicationException($"Container {containerName} does not exist");

            var blobList = blobsPerContainer[containerName];
            var index = blobList.FindIndex(b => b.Name == blobName);

            if (index == -1)
                throw new ApplicationException($"Could not find {blobName}  in {containerName}");

            blobList.RemoveAt(index);
        }

        public void DeleteContainer(string containerName)
        {
            if (!blobsPerContainer.ContainsKey(containerName))
                throw new ApplicationException($"Cannot delete container {containerName} because it does not exist");

            storageContainers.RemoveAll(c => c.Name == containerName);
            blobsPerContainer.Remove(containerName);
        }

        public BlobModel GetBlobContents(string containerName, string blobName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StorageContainerModel> GetContainers()
        {
            return storageContainers.AsEnumerable();
        }

        public string GetStorageAccountName()
        {
            return "Test Account";
        }

        public IEnumerable<BlobInfoModel> ListBlobsInContainer(string containerName)
        {
            if (!blobsPerContainer.ContainsKey(containerName))
                throw new ApplicationException($"Container {containerName} does not exist");

            return blobsPerContainer[containerName].AsEnumerable();
        }

        public void UploadBlob(string containerName, string blobName, string contentType, Stream content)
        {
            if (!blobsPerContainer.ContainsKey(containerName))
                throw new ApplicationException($"Container {containerName} does not exist");

            var blobList = blobsPerContainer[containerName];
            BlobInfoModel blob = new BlobInfoModel() { Name = blobName, CreatedOn = DateTime.Now, Size = content.Length, ContentType = contentType};
            blobList.Add(blob);
        }
    }
}
