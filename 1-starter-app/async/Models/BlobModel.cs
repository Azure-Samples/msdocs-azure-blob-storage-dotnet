using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{

    /// <summary>
    /// Represents the contents of a blob stored in Azure Storage
    /// </summary>
    public class BlobModel
    {

        public string Name { get; set; }

        public string ContentType { get; set; }

        public Stream Content { get; set; }

    }
}
