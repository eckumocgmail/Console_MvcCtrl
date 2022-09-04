using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tools.Hosted.Utils
{
    public class UtilsUnit : TestingUnit
    {
        public UtilsUnit()
        {
            Push(new GITTest());
            Push(new DLLTest());
            Push(new EFTest());
            Push(new PowerShellTest());
            Push(new CMDTest());
            
            Push(new DOSTest());
            Push(new DontnetTest());
            
            Push(new FilesTest());
            
            Push(new JavaTest());
            Push(new JsonTest());
            Push(new NodeJSTest());
            
            Push(new USBTest());
            Push(new WinTest());
        }
    }
}
