using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    abstract class AppException: Exception
    {
        public string MethodName { get; }
        public object[] InvokeArgs { get; }
        public AppException(string method, params object[] args)
        {
            this.MethodName = method;
            this.InvokeArgs = args;
        }
    }
}
