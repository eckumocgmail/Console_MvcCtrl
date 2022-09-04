using ConsoleApp.console_app_src.Streaming;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace console_app_proj.console_app_src
{


    /*
    public class ProcessScope: IStreaming
    {

        static string[] PathDirectories { get => Environment.GetEnvironmentVariable("path").Split(";"); }
        static bool IsShutdown { get => Environment.HasShutdownStarted; }
        static bool IsX64 { get => Environment.Is64BitOperatingSystem; }
        static string LineSeparator { get => Environment.NewLine; }
        static int ExitCode { get => Environment.ExitCode; set => Environment.ExitCode = value; }
        static int ThreadID { get => Environment.CurrentManagedThreadId; }
        static string Command { get => Environment.CommandLine; }
        static int ProcessID { get => Environment.ProcessId; }
        static string StackTrace { get => Environment.StackTrace; }


        private Assembly CurrentAssembly;
        private IStreaming StreamingProxy;


        public void WriteLine(string text) => System.Console.WriteLine(text);
        public string ReadLine() => System.Console.ReadLine();
        public async Task<string> ReadLineAsync() => await Task.Run(() => {
            return System.Console.ReadLine();
        });
        public async Task ClearAsync() => await Task.Run(() => { System.Console.Clear(); });
        public void Clear() => System.Console.Clear();
        public async Task WriteLineAsync(string text) => await Task.Run(() => {
            System.Console.WriteLine(text);
        });








 
        public static ProcessScope GetWithProxy(IStreaming proxy)
        {
            return new ProcessScope()
            {
                StreamingProxy = proxy
            };
        }

       

        string Account
        {
            get => Environment.UserDomainName +
            @"." + Environment.MachineName +
            @"." + Environment.NewLine + Environment.UserName;
        }

        string GetPlaform()
        {
            if (OperatingSystem.IsAndroid()) return "android";
            if (OperatingSystem.IsWindows()) return "windows";
            if (OperatingSystem.IsBrowser()) return "browser";
            if (OperatingSystem.IsLinux()) return "linux";
            if (OperatingSystem.IsIOS()) return "ios";
            if (OperatingSystem.IsTvOS()) return "tvos";
            if (OperatingSystem.IsMacOS()) return "macos";
            throw new Exception("Не удалось определить платформу");
        }

        private static ProcessScope Instance;


        public static ProcessScope Init()
        {
            var scope = new ProcessScope();
             
            return scope;
        }

        public static void Test()
        {
            var scope = new ProcessScope();
            
        }

        public static ProcessScope Get()
        {
            if (Instance == null)
            {
                Instance = Init();
            }
            return Instance;
        }


        



        
    }*/

}
