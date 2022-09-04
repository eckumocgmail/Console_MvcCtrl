using System;
using System.Collections.Generic;
using System.Text;

namespace eckumoc
{
    public class USBDeviceDescriptor
    {
        public int bLength { get; set; }
        public int bDescriptorType { get; set; }
        public int bcdUSB { get; set; }
        public int bDeviceClass { get; set; }
        public int bDeviceSubClass { get; set; }
        public int bDeviceProtocol { get; set; }
        public int bMaxPacketSize0 { get; set; }
        public int idVendor { get; set; }
        public int idProduct { get; set; }
        public int bcdDevice { get; set; }
        public int iManufacturer { get; set; }
        public int iProduct { get; set; }
        public int iSerialNumber { get; set; }
        public int bNumConfigurations { get; set; }     
    }
}
