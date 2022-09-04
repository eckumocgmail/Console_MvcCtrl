using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eckumoc_common_api.MvcModule.MvcInterfaces
{
    //[Route("/api/[controller]/[action]")]
    public abstract class BaseHttpMethodController: Controller
    {

        public virtual object Index() => OnGet();
        public abstract Task<IActionResult> OnGet();
        public abstract Task<IActionResult> OnPost();
        public abstract Task<IActionResult> OnPut();
        public abstract Task<IActionResult> OnPatch();
        public abstract Task<IActionResult> OnOptions();
   
    }
}
