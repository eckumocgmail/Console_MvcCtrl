using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetCore
{ 
    public interface INetCoreApp
    {
        public string Build();
        public string Pack();
        public string Unpack();
        public string Run(string url);

    }
}
