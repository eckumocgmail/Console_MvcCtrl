using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eckumoc_common_api.Views.Navigation
{
    public interface INavigation: IFileNavigation
    {
        public enum ResourceType
        {
            File, Localhost, Www
        }
        

        public ResourceType GetConnectionType(string url);

        public bool IsFile(string url);
        public bool InOrigin(string url);
        public bool IsCors(string url);
    }
}
