using COM;

using DetailsAnnotationsNS;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eckumoc_common_api.CommonAttrsTest
{
    public class TestCustomAttrs: TestingElement
    {
        public string Icon { get; set; }
        protected override void OnTest()
        {
            AttrUtils.ForType(GetType()).ToJsonOnScreen().WriteToConsole();
            //CustomAttrs.AddTypeAttr(GetType(), nameof(EntityIconAttribute), "menu");
            //AttrUtils.ForType(GetType()).ToJsonOnScreen().WriteToConsole();
        }
    }
}
