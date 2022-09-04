using NetCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Net
{
    public class NetCoreApp : INetCoreApp
    {
        public NetCoreApp(string location)
        {

        }

        public string Build()
        {
            throw new NotImplementedException();
        }

        public string Pack()
        {
            throw new NotImplementedException();
        }

        public string Unpack()
        {
            throw new NotImplementedException();
        }

        public string Run(string url)
        {
            throw new NotImplementedException();
        }
    }
}
