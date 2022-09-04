using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.console_app_src.Streaming
{
    public class QueueSreaming: IStreaming
    {
        private Queue<string> histore = new Queue<string>();



        public void WriteLine(string text) => histore.Enqueue(text);

        public string ReadLine() 
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void WriteLineAsync(string text)
        {
            throw new NotImplementedException();
        }

        public string ReadLineAsync()
        {
            throw new NotImplementedException();
        }

        public void ClearAsync()
        {
            throw new NotImplementedException();
        }

        Task IStreaming.WriteLineAsync(string text)
        {
            throw new NotImplementedException();
        }

        Task<string> IStreaming.ReadLineAsync()
        {
            throw new NotImplementedException();
        }

        Task IStreaming.ClearAsync()
        {
            throw new NotImplementedException();
        }
    }
}
