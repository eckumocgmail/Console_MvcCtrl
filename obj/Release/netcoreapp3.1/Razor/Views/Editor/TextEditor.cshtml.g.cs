#pragma checksum "D:\Projects-Console\ConsoleMvc\Views\Editor\TextEditor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a5a85b29056caae332809e68d69b2e2026360e82"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Editor_TextEditor), @"mvc.1.0.view", @"/Views/Editor/TextEditor.cshtml")]
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
#line 1 "D:\Projects-Console\ConsoleMvc\Views\_ViewImports.cshtml"
using eckumoc_common_api;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects-Console\ConsoleMvc\Views\_ViewImports.cshtml"
using eckumoc_common_api.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a5a85b29056caae332809e68d69b2e2026360e82", @"/Views/Editor/TextEditor.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"75a08c2bdcc481cc35fc59c4676b36739274acf6", @"/Views/_ViewImports.cshtml")]
    public class Views_Editor_TextEditor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "D:\Projects-Console\ConsoleMvc\Views\Editor\TextEditor.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<script>
    function layout(inner, outer)
    {
        function updatePosition(){
            inner.style.position = 'absolute';
            inner.style.left = outer.offsetLeft+'px';
            inner.style.top = outer.offsetTop+'px';
            inner.style.width = outer.offsetWidth+'px';
            inner.style.height = outer.offsetHeight+'px';
        }
        document.addEventListener('scroll',updatePosition);
        window.addEventListener('resize',updatePosition);
        window.dispatchEvent(new Event('resize'));
    }
</script>

<div>
    <div id=""pane"" style=""width: 100%; height: 320px;"" class=""btn btn-primary"">
    <button onclick=""layout(button,pane)"" style=""position: absolute; width: 160px; height: 160px;"" class=""btn btn-danger""> position </button>    
    </div>
    <button id=""button"" onclick=""layout(button,pane)"" style=""width: 160px; height: 160px;"" class=""btn btn-danger""> 1 </button>
    <hr/>

</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
