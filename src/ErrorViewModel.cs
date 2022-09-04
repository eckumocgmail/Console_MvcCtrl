using System;
namespace eckumoc_common_api.Models { }
namespace ApplicationCore.Converter.Models 
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
