using eckumoc_common_api.CommonAttrsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTests
{
    class CommonAttrsTests : TestingUnit
    {
        public CommonAttrsTests()
        {
            Push(new TestCustomAttrs());
            Push(new TestDataAttributes());
        }
    }
    
    class CommonComponentTests : TestingUnit
    {
        public CommonComponentTests()
        {
             
        }
    }
    class CommonExtensionsTests : TestingUnit
    {
        public CommonExtensionsTests()
        {
        }
    }
    class CommonModelTests : TestingUnit
    {
        public CommonModelTests()
        {
        }
    }
    class CommonStreamingTests : TestingUnit
    {
        public CommonStreamingTests()
        {
        }
    }

    public class CommonUnit: TestingUnit
    {
        public static void OnTesting()
        {
            var unit = new CommonUnit();
            unit.DoTest().ToDocument().WriteToConsole();
        }
        public CommonUnit()
        {
            Push(new CommonUtilsUnit());
            Push(new CommonStreamingTests());
            Push(new CommonAttrsTests());
            Push(new CommonCollectionsTest());
            Push(new CommonComponentTests());
            Push(new CommonExtensionsTests());
            Push(new CommonModelTests());

            Push(new CommonCollectionsUnit());
            Push(new CommonExecuterUnit());
            Push(new CommonModelUnit());
            Push(new CommonProcessingUnit());
            //Push(new CommonBuilderUnit());
            Push(new CommonSerializationUnit());
            
        }

        public static void Test()
        {
            var unit = new CommonUnit();
            unit.DoTest().ToDocument().WriteToConsole();
        }
    }
}
