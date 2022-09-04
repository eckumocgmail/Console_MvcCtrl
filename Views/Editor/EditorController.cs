using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eckumoc_common_api.Views.Editor
{
    public class EditorController: Controller
    {


        public IActionResult Index() => View("TextEditor");
        public IActionResult TextEditor() => View();
        public IActionResult XmlEditor() => View();
    }
}
