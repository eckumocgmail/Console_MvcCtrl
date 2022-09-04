using CommonTests.CommonProcessing;
using ConsoleApp.console_app_src.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>
    /// Предоставляет интерфейс навигации по файловой системе и выбор файлв с заданным шаблоном
    /// </summary>
    public class FileNavigator : INavigator, IStreaming
    {
        public string Filters { get; set; } = "*.exe";
        private string CurrentLocation = System.IO.Directory.GetCurrentDirectory();

        private string RootLocation = @"d:";
        public string Location { get => CurrentLocation; set => CurrentLocation = value; }
        public IStreaming Connection { get; }
        private Func<string, string, IEnumerable<string>> GetDirNames =
            (location, pattern) => System.IO.Directory.GetDirectories(location, pattern).Select(p => p.Substring(location.Length + 1)).ToArray();
        private Func<string, string, IEnumerable<string>> GetFileNames =
            (location, pattern) =>
            {
                var all = System.IO.Directory.GetFiles(location, pattern).ToArray();
                //System.IO.Directory.GetFiles(location, pattern).Select(p => p.Substring(location.Length + 1)).ToArray();
                return all;
            };

        public FileNavigator(): this(new ConsoleStreaming())
        {

        }

     
      
        public FileNavigator(IStreaming Connection) : base( )
        {
            this.Connection = Connection;
        }


        public static void Test()
        {
            var nav = new FileNavigator();
            nav.SelectFile("*.exe");
        }

        public static void OnTest(string[] args)
        {
            var nav = new FileNavigator();
            nav.SelectFile("*.exe");
        }

        public string[] Path()
        {
            var set = new List<string>();
            set.AddRange(GetFileNames(CurrentLocation, this.Filters));
            set.AddRange(GetDirNames(CurrentLocation, this.Filters));
            return set.ToArray();
        }


        public void Next(string path)
        {
            SelectFile(path);
        }
        public async Task ConnectAsync(IStreaming caller)
            => await this.Connection.WriteLineAsync(await caller.ReadLineAsync());
            
        

        public static void OnRun( ) => Run(new string[0]);

        public static void Run(string[] args)
        {
            var navigator = new FileNavigator( );
            navigator.Next(System.IO.Directory.GetCurrentDirectory());
            navigator.SelectFile(navigator.Filters);            
        }





        /// <summary>
        /// Выбор одной из под директорий
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="path">Путь к директориям</param>
        /// <returns>результат выбора</returns>
        public virtual string SelectDirectory(string message, string path)
        {
            var options = System.IO.Directory.GetDirectories(path).Select(p => p.Substring(path.Length + 1));
            string input = "";
            do
            {
                this.WriteLine(message);
                this.WriteLine("..");
                foreach (var dir in options)
                {
                    this.WriteLine($"{dir}");
                }
                input = this.ReadLine();
            } while (options.Contains(input) == false);
            return input;
        }




        public async Task<string> SelectDirectoryAsync(string message, string path)
        {
            
            var options = System.IO.Directory.GetDirectories(path).Select(p => p.Substring(path.Length + 1));
            string input = "";
            await this.WriteLineAsync(message);
            await this.WriteLineAsync("..");
            foreach (var dir in options)
            {
                await this.WriteLineAsync($"{dir}");
            }
            return await this.ReadLineAsync();
                
            
        }

        /// <summary>
        /// Выбор одной из под директорий
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="path">Путь к директориям</param>
        /// <returns>результат выбора</returns>
        public string SelectFile(string pattern)
        {
            this.CurrentLocation = RootLocation;

            string result = "";
            string message = "";
            string input = "";
            do
            {
                this.Clear();
                this.WriteLine(message);
                this.WriteLine(CurrentLocation);
                this.WriteLine(".");
                this.WriteLine("..");
                foreach (var dir in GetDirNames(CurrentLocation, "*"))
                {
                    this.WriteLine($"{dir}");
                }
                var files = GetFileNames(CurrentLocation, this.Filters);
                foreach (var file in files)
                {
                    this.WriteLine($"{file}");
                }
                input = this.ReadLine();


                if (input == ".")
                {
                    CurrentLocation = RootLocation;
                    continue;
                }

                if (input == "..")
                {
                    CurrentLocation = CurrentLocation.Substring(0, CurrentLocation.LastIndexOf("\\"));
                    continue;
                }

                string path = CurrentLocation + "\\" + input;
                if (files.Contains(input) == false && System.IO.Directory.Exists(path) == false)
                {
                    message = $"Не существует варианта {input}";
                    continue;
                }
                message = "";
                if (System.IO.Directory.Exists(path))
                {
                    CurrentLocation = path;
                    continue;
                }
                else
                {
                    result = path;
                    return result;
                }
            } while (string.IsNullOrEmpty(result));
            return result;
        }

        public void WriteLine(string text) => this.Connection.WriteLine(text);

        public string ReadLine() => this.Connection.ReadLine();

        public void Clear() => this.Connection.Clear();

        public Task WriteLineAsync(string text) => this.Connection.WriteLineAsync(text);

        public Task<string> ReadLineAsync() => this.Connection.ReadLineAsync();

        public Task ClearAsync() => this.Connection.ClearAsync();

        public Task NextAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
