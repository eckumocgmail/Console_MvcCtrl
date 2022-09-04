using DataADO;

using DataCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eckumoc_common_api.CommonModule.NetworkModule.DataModule.Resources
{
    public class DataTableServiceTest: TestingElement
    {
        protected override void OnTest()
        {
            ADODbConnectorsTest.Run();
            ADOExecutorTest.Run();
            ADODbMetadataTest.Run();
            
        }
    }
}
