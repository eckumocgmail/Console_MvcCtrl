using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MVC.Controllers;

using NameSpaceModule;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eckumoc_common_api.CommonModule.NetworkModule
{

    [Microsoft.AspNetCore.Components.Route("/api")]
    [Microsoft.AspNetCore.Mvc.Route("/api")]
    public class NetworkController : ServiceMethodController<NameServiceProvider>
    {
        public NetworkController()
        {
        }

        [HttpGet("/api/index")]
        public string WhoAreYou()
        {
            return GetType().Name;
        }
    }
}
