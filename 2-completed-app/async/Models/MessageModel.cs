﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{
    [Serializable]
    public class MessageModel
    {

        public MessageLevel Level { get; set; }

        public string Message { get; set; }

    }
}
