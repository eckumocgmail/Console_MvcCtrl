using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WatchModule
{
 
    /// <summary>
    /// Выполнение действия по событиям файловой системы
    /// </summary>
    public class FileWatcher: FileSystemWatcher
    {       
        /// <summary>
        /// Время доступа в спец. формате FileTimeUtc
        /// </summary>
        private IDictionary<string, long> text = new Dictionary<string, long>();

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public FileWatcher() : this(System.IO.Directory.GetCurrentDirectory()) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="wrk">путь к директории</param>
        public FileWatcher( string wrk ):  base( wrk )
        {                     
            this.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;
            this.IncludeSubdirectories = true;
        }

 

        /// <summary>
        /// Связывание действия с событиями файловой системы
        /// </summary>
        /// <param name="filter">фильтр файлов</param>
        /// <param name="todo">операция выполняемая по событию</param>
        public void Watch(Action<FileSystemEventArgs> todo, string filter = "*.*")
        {                  
            this.Changed += (sender,evt)=> 
            {                                
                long state = System.IO.File.GetLastAccessTime(base.Path).ToFileTimeUtc();
                if(text.ContainsKey(evt.FullPath)==false || state != text[evt.FullPath])
                {                                         
                    todo(evt);
                }
                text[evt.FullPath] = state;
            };                                            
            this.EnableRaisingEvents = true;              
        }                               
    }


    /// <summary>
    /// Системная утилита командной строки
    /// </summary>
    public class Cmd
    {
        public string Execute(string command)
        {
            Console.WriteLine("exec=>" + command);
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            string response = process.StandardOutput.ReadToEnd();
            Console.WriteLine(response);
            return response;
        }
    }
}
