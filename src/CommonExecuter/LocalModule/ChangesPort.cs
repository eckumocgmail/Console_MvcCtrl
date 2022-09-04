using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Public.Builder
{
    public class ChangesPort : IHostedService,IDisposable
    {
        public static ConcurrentQueue<Action> Wainting = new ConcurrentQueue<Action>();
        public static ConcurrentQueue<Action> Completed = new ConcurrentQueue<Action>();
        public static ConcurrentQueue<Action> Rollback = new ConcurrentQueue<Action>();
        //public static ConcurrentQueue<Action> EventsQueue = new ConcurrentQueue<Action>();
















        /// <summary>
        /// Watch for changes in LastAccess and LastWrite times, 
        /// and the renaming of files or directories.
        /// </summary>       
        private static string service;

     
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            ChangesPort.Wainting.Enqueue(() => {
                Debug.WriteLine(e.FullPath);
                Execute($"{service} {e.FullPath} {e.ChangeType}");
            });
        }





        private static void OnRenamed(object source, RenamedEventArgs e) =>
            Execute($"{service} {e.OldFullPath} renamed to {e.FullPath}");

        /// <summary>
        /// Create a new FileSystemWatcher and set its properties.      
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filter"> *.txt </param>
        /// <param name="service"></param>
        public static void Watch(CancellationToken cancellationToken, string dir, string filter, string service)
        {
            ChangesPort.service = service;
            using (FileSystemWatcher watcher = new FileSystemWatcher(dir))
            {
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                        | NotifyFilters.LastWrite
                                        | NotifyFilters.FileName
                                        | NotifyFilters.DirectoryName;
                watcher.Filter = filter;

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;


                while (cancellationToken.IsCancellationRequested != true) Thread.Sleep(100);
            }
        }

        public static string Execute(string command)
        {
            System.Console.WriteLine("exec=>" + command);
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            process.WaitForExit();
            string response = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(response);
            return response;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var t = Task.Run(()=> {
                Action message;
                while (cancellationToken.IsCancellationRequested==false)
                {                    
                    Wainting.TryDequeue(out message);
                    if(message != null)
                    {
                        OnMessage(message);
                    }
                    Thread.Sleep(100);
                }
            });
            throw new Exception("Нужно поправить параметр ContentRoot в ChangePort.StartAsync");
            /*return Task.Run(()=> {
                System.Console.WriteLine("started");
                Watch(cancellationToken, / *Program.Env.ContentRootPath* /"", "*.*", "BusinessProcess");
            });*/
        }



        private void OnMessage(Action message)
        {
            message();
        }




        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => {
            });

        }

        public void Dispose()
        {
            
        }
    }
}
