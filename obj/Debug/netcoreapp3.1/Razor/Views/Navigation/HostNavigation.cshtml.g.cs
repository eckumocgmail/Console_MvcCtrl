#pragma checksum "D:\Projects\Console_MvcCtrl\Views\Navigation\HostNavigation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a357d0822ab4cf2ddb344eef758f0b6444621c38"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Navigation_HostNavigation), @"mvc.1.0.view", @"/Views/Navigation/HostNavigation.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Projects\Console_MvcCtrl\Views\Navigation\HostNavigation.cshtml"
using System.Reflection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects\Console_MvcCtrl\Views\Navigation\HostNavigation.cshtml"
using ApplicationCore.Converter.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a357d0822ab4cf2ddb344eef758f0b6444621c38", @"/Views/Navigation/HostNavigation.cshtml")]
    #nullable restore
    public class Views_Navigation_HostNavigation : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 6 "D:\Projects\Console_MvcCtrl\Views\Navigation\HostNavigation.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 10 "D:\Projects\Console_MvcCtrl\Views\Navigation\HostNavigation.cshtml"
  
    object app = new MyActionModel();
    CommonNode<object> pnode = new CommonNode<object>("Api",app,null) ;

    foreach(var ctrl in Assembly.GetExecutingAssembly().GetControllers())
    {
        var next = new CommonNode<object>(ctrl.GetType().GetTypeName(), (object)ctrl, pnode);
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("<partial name=\"_Node\" model=\"pnode\"/>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591