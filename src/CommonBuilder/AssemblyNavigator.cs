using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>
    /// Предоставляет интерфейс навигации по файловой системе и выбор файлв с заданным шаблоном
    /// </summary>
    class AssemblyNavigator: FileNavigator
    {
        private Assembly _assembly = Assembly.GetExecutingAssembly();
        private string RootLocation = @"d:\projects_ftp";
        private string CurrentLocation = System.IO.Directory.GetCurrentDirectory();

        private Func<string,string,IEnumerable<string>> GetDirNames = 
            (location,pattern)=>System.IO.Directory.GetDirectories(location, pattern).Select(p => p.Substring(location.Length + 1)).ToArray();
        private Func<string, string, IEnumerable<string>> GetFileNames =
            (location, pattern) => System.IO.Directory.GetFiles(location, pattern).Select(p => p.Substring(location.Length + 1)).ToArray();


        public AssemblyNavigator(   ): base( )
        {            
        }

   



        /// <summary>
        /// Выбор одной из под директорий
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="path">Путь к директориям</param>
        /// <returns>результат выбора</returns>
        public override string SelectDirectory(string message, string path)
        {
            var options = System.IO.Directory.GetDirectories(path).Select(p => p.Substring(path.Length + 1));
            string input = "";
            do
            {
                Console.WriteLine(message);
                Console.WriteLine("..");
                foreach (var dir in options)
                {
                    Console.WriteLine($"{dir}");
                }
                input = Console.ReadLine();
            } while (options.Contains(input) == false);
            return input;
        }

        /// <summary>
        /// Выбор одной из под директорий
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="path">Путь к директориям</param>
        /// <returns>результат выбора</returns>
        public string SelectFiles(string pattern)
        {
            this.CurrentLocation = RootLocation;

            string result = "";
            string message = "";
            string input = "";
            do
            {
                Console.Clear();
                Console.WriteLine(message);
                Console.WriteLine(CurrentLocation);
                Console.WriteLine(".");
                Console.WriteLine("..");
                foreach (var dir in GetDirNames(CurrentLocation, "*"))
                {
                    Console.WriteLine($"{dir}");
                }
                var files = GetFileNames(CurrentLocation, pattern);
                foreach (var file in files)
                {
                    Console.WriteLine($"{file}");
                }
                input = Console.ReadLine();


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

                string path = CurrentLocation + "\\"+input;
                if(files.Contains(input) == false && System.IO.Directory.Exists(path) == false)
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

    }
}
