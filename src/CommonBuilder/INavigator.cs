using ConsoleApp.console_app_src.Streaming;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public interface INavigator: IStreaming
    {
        public string[] Path();
        public string Location { get; set; }
        public void Next(string path);
        public Task NextAsync(string path);
        public Task ConnectAsync(IStreaming caller);
    }
}