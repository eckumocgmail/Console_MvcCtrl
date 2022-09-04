using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUnit.CommonUnit;
using DataADO;
using eckumoc_common_api.CommonAttrsTest;
using eckumoc_common_api.CommonModule.NetworkModule.DataModule.Resources;
using SpbPublicLibsUnit.Encoder;
using StartupModule;
using TheMovieDatabase;
using tools.Hosted;
using tools.Hosted.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NameSpaceModule;
using System.IO;

namespace StartupModule
{
    class LocalhostUnit : TestingUnit
    {

        public static void Test()
        {
            var unit = new LocalhostUnit();
            unit.DoTest().ToDocument();
        }

        public LocalhostUnit()
        {
            Push(new HostedAppUnitTest());
            Push(new HostedControllerUnitTest());
            Push(new HostedErpUnitTest());
            Push(new HostUtilsUnitTest());

            (new List<object>() {

                new NetworkMonitorTest(),
                new USBTest(),
                new WinTest(),
                new TestEncode(),
                new TestExpressions(),

                   new TestCustomAttrs(),new TestDataAttributes(),new ServerAppTest(),new CommonCollectionsTest(), new CMDTest(),

                new FileResourceTest(),
                new DataTableServiceTest(),
                new ADODbConnectorsTest(),
                new ADOExecutorTest(),
                new ADODbMetadataTest(),
                new ADODbMigBuilderTest(),
                new NetworkUnit(),
                new MdbUnit(),
                new DLLTest(),
                new DOSTest(),
                //new GITTest(),
                new JavaTest(),
                new NodeJSTest(),
                new PowerShellTest(),
                new DontnetTest(),
                new EFTest(),
                new FilesTest(),
                new HostedAppFilesTest(),
                new HostedAppUnitTest(),
                new HostedControllerUnitTest(),
                new HostedErpUnitTest(),
                new HostUtilsUnitTest(),
                new JsonTest() }).ForEach(test => Push((TestingElement)test));
        }
    }
}
