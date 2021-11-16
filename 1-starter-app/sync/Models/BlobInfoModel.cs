using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{
    public class BlobInfoModel
    {

        public string Name { get; set; }

        public string BlobType { get; set; }

        public string ContentEncoding { get; set; }


        public string ContentType { get; set; }

        public long? Size { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public string AccessTier { get; set; }

        public IDictionary<string, string> Tags { get; set; }

    }
}
