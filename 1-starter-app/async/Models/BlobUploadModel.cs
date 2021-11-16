using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{
    public class BlobUploadModel
    {

        [Required]
        [RegularExpression(@"(\w|\d|\.|\/|\\){1,96}")]
        public string Name { get; set; }

        public IFormFile UploadFile { get; set; }
    }
}
