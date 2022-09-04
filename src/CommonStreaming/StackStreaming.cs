using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.console_app_src.Streaming
{
    public class StackStreaming: IStreaming
    {

        private System.Collections.Concurrent.ConcurrentStack<string> memory = 
            new System.Collections.Concurrent.ConcurrentStack<string>();
        
        
        public void WriteLine(string text)
        {
            memory.Push(text);
        }

        public string ReadLine()
        {
            string result;
            memory.TryPop(out result);
            return result;
        }


        public void Clear() => memory.Clear();

        public Task WriteLineAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReadLineAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }
    }
}
