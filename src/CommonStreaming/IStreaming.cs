 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.console_app_src.Streaming
{
    public interface IStreaming
    {
       

        void WriteLine(string text);
        string ReadLine();
        void Clear();


        Task WriteLineAsync(string text);

        Task<string> ReadLineAsync();

        Task ClearAsync();
    }
}
