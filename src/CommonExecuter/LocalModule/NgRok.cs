using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class NgRok
{
    private Process _process;
    private int _port { get; set; }
    public NgRok(int port)
    {
        _port = port;
    }

    public void Start()
    {
        Console.WriteLine("перенаправление порта " + _port + " в интернет ");
        _process = Process.Start("powershell", @"/C D:\Programs\NgRok\ngrok.exe http " + _port);                     
    }

    public void Stop()
    {
            
        //_process.Stop();
        Console.WriteLine(_process);
    }
    



    public void Run()
    {
        StartInBackground();
    }
    public void StartInBackground()
    {
        Console.WriteLine("перенаправление порта " + _port + " в интернет ");
        Thread work = new Thread(new ThreadStart(() => {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + "ngrok http "+_port);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            _process = System.Diagnostics.Process.Start(info);           
            string line = null;
            while(((line= _process.StandardOutput.ReadLine()))!=null)
            {
                Console.WriteLine(line);
                Console.WriteLine(line);
                Console.WriteLine(line);
                Console.WriteLine(line);
                Console.WriteLine(line);
                Console.WriteLine(line);
                Console.WriteLine(line);
                    
                if(line.IndexOf("Forwarding")!=-1){
                    Console.WriteLine(line);
                }
                    
            }
                              
        }));
        work.IsBackground = true;
        work.Start();
            
    }
}