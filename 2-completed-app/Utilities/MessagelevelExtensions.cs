using AzureBlobStorageDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Utilities
{
    public static class MessagelevelExtensions
    {


        public static string ToCssClass(this MessageLevel level)
        {
            switch (level)
            {
                case MessageLevel.Danger:
                    return "alert-danger";
                case MessageLevel.Warning:
                    return "alert-warning";
                case MessageLevel.Success:
                    return "alert-success";
                case MessageLevel.Primary:
                    return "alert-primary";
                case MessageLevel.Secondary:
                    return "alert-secondary";
                default:
                    return "alert-info";
            }
        }

    }
}
