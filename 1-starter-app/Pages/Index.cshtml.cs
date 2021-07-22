using AzureBlobStorageDemo.Models;
using AzureBlobStorageDemo.Services;
using AzureBlobStorageDemo.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBlobStorageService _blobStorageService;

        public IndexModel(ILogger<IndexModel> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }


        public IEnumerable<StorageContainerModel> Containers { get; set; }
        public string SelectedContainer { get; set; }        
        public IEnumerable<BlobInfoModel> BlobsInContainer { get; set; }


        public void OnGet(string container = null)
        {
            Containers = _blobStorageService.GetContainers()
                .OrderBy(c => c.Name);

            if ( Containers.Any(c => c.Name == container))
            {
                SelectedContainer = container;
                BlobsInContainer = _blobStorageService.ListBlobsInContainer(container).OrderBy(b => b.Name);
            }
            else if (container != null)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = $"Container {container} does not exist" };
                TempData.Put<MessageModel>("Message", message);
            }
        }
        
        public IActionResult OnPostCreateContainer(string containerName)
        {
            try
            {
                _blobStorageService.CreateContainer(containerName);
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message};
                TempData.Put<MessageModel>("Message", message);
            }          
            return RedirectToPage("index", "Get", new { container = containerName });
        }


        public IActionResult OnPostRemoveContainer(string containerName)
        {
            try
            {
                _blobStorageService.DeleteContainer(containerName);
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { containerName = ""});
        }


        public IActionResult OnGetDownloadBlob(string containerName, string blobName)
        {
            try
            {
                BlobModel blob = _blobStorageService.GetBlobContents(containerName, blobName);
                return new FileStreamResult(blob.Content, blob.ContentType) { FileDownloadName = blob.Name };
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { container = SelectedContainer });
        }


        public IActionResult OnPostRemoveBlob(string containerName, string blobName)
        {
            try
            {
                _blobStorageService.DeleteBlob(containerName, blobName);
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { container = containerName });
        }



        public IActionResult OnPostUploadBlob(string containerName, string blobName, IFormFile uploadFile)
        {
            try
            {
                using (var stream = uploadFile.OpenReadStream())
                {
                    _blobStorageService.UploadBlob(containerName, blobName, uploadFile.ContentType, stream);
                }
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { container = containerName });
        }



    }
}
