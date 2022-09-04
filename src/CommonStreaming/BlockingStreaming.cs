using ConsoleApp.console_app_src.Streaming;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming
{
    public class BlockingStreaming: IStreaming
    {
        private System.Collections.Concurrent.ConcurrentQueue<string> memory = 
            new System.Collections.Concurrent.ConcurrentQueue<string>();

    



        /*Action todo = () => { };
        var t1 = new Thread(() => {
            while (true)
            {
                memory.Add("asdasd");
                Thread.Sleep(1000);
            }
        });*/
        

        
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public string ReadLine()
        {
            return "";
        }

        public void Clear()
        {
            memory.Clear();
        }

        public Task WriteLineAsync(string text)
        {
            return Task.Run(() => {                               
                memory.Enqueue(text);                                                   
            });
        }

        public async Task<string> ReadLineAsync()
        {
            return await Task.Run(() => {
                string next;
                while (true)
                {                    
                    memory.TryDequeue(out next);
                    if (string.IsNullOrEmpty(next)==false)
                    {
                        break;
                    }
                }
                return next;
            });
        }

        public async Task ClearAsync()
        {
            await Task.Run(()=> {
                memory.Clear();
            });
        }
    }
}
