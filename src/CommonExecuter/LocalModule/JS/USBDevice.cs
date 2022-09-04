using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace eckumoc
{
    public class USBDevice: ISerializable
    {
        public int busNumber { get; set; }
        public int deviceAddress { get; set; }
        public USBDeviceDescriptor deviceDescriptor { get; set; }
        public List<int> portNumbers { get; set; } = new List<int>();


        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
