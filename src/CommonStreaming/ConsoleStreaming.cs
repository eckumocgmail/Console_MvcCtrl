using ConsoleApp.console_app_src.Streaming;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTests.CommonProcessing
{
    public class ConsoleStreaming: IStreaming
    {
        public void WriteLine(string text) => Console.WriteLine(text);

        public string ReadLine() => Console.ReadLine( );

        public void Clear() => Console.Clear( );

        public Task WriteLineAsync(string text) => Task.Run(() => { Console.WriteLine(text); });

        public Task<string> ReadLineAsync() => Task.Run(() => { return Console.ReadLine( ); });

        public Task ClearAsync() => Task.Run(() => { Console.Clear( ); });
    }
}
