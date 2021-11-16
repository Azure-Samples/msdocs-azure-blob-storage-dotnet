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


        public IAsyncEnumerable<StorageContainerModel> Containers { get; set; }
        public string SelectedContainer { get; set; }        
        public IAsyncEnumerable<BlobInfoModel> BlobsInContainer { get; set; }


        public async Task OnGet(string container = null)
        {
            //Containers = (await _blobStorageService.GetContainersAsync().ToListAsync()).OrderBy(c => c.Name);
            Containers = _blobStorageService.GetContainersAsync().OrderBy(c => c.Name);

            if ( await Containers.AnyAsync(c => c.Name == container))
            {
                SelectedContainer = container;
                BlobsInContainer = _blobStorageService.ListBlobsInContainerAsync(container).OrderBy(b => b.Name);
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
                _blobStorageService.CreateContainerAsync(containerName);
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
                _blobStorageService.DeleteContainerAsync(containerName);
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { containerName = ""});
        }


        public async Task<IActionResult> OnGetDownloadBlob(string containerName, string blobName)
        {
            try
            {
                BlobModel blob = await _blobStorageService.GetBlobContentsAsync(containerName, blobName);
                return new FileStreamResult(blob.Content, blob.ContentType) { FileDownloadName = blob.Name };
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { container = SelectedContainer });
        }


        public async Task<IActionResult> OnPostRemoveBlob(string containerName, string blobName)
        {
            try
            {
                await _blobStorageService.DeleteBlobAsync(containerName, blobName);
            }
            catch (ApplicationException ex)
            {
                MessageModel message = new MessageModel() { Level = MessageLevel.Warning, Message = ex.Message };
                TempData.Put<MessageModel>("Message", message);
            }
            return RedirectToPage("index", "Get", new { container = containerName });
        }



        public async Task<IActionResult> OnPostUploadBlob(string containerName, string blobName, IFormFile uploadFile)
        {
            try
            {
                using (var stream = uploadFile.OpenReadStream())
                {
                    await _blobStorageService.UploadBlobAsync(containerName, blobName, uploadFile.ContentType, stream);
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
