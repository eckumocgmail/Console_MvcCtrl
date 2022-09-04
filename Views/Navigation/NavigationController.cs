using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eckumoc_common_api.Views.Navigation
{
    public class NavigationController : Controller
    {
        private readonly INavigation _navigationService;

        public NavigationController(INavigation navigationService)
        {
            _navigationService = navigationService;
        }


        public IActionResult Index() => Redirect("/Navigation/HostNavigation");
        public IActionResult HostNavigations() => Redirect("/Navigation/HostNavigation");
        public IActionResult FileNavigation() => View();

        public INavigation.ResourceType GetType(string url)
            => _navigationService.GetConnectionType(url);

        public bool IsFile(string url) => _navigationService.IsFile(url);
        public bool InOrigin(string url) => _navigationService.IsFile(url);
        public bool IsCors(string url) => _navigationService.IsFile(url);
 
    }
}
