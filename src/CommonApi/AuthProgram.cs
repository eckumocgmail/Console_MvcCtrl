using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using static EcKuMoC;

 

namespace eckumoc_netcore_auth
{
    public class Program: EcKuMoC
    {
        static void Run(string[] args)
        {            
            Environment.SetEnvironmentVariable("comspec","");
            if (args.Length == 0)
            {
                if(Environment.UserInteractive)
                {

                    while(true)
                    {
                        OnStart(InteractiveInputParams(), GetDefaulKeys());
                    }
                }
                else
                {
                    Help();
                }
                
            }
            else
            {
                string input = ReadAsJson(args);
                Console.WriteLine("input="+input);
                var ProcessParameters = ParseJson(input);
                var keys = (ProcessParameters.ContainsKey("keys")) ? ProcessParameters["keys"].Trim().Split(" ") : GetDefaulKeys();
                OnStart(ProcessParameters, keys);
            }
        }

        private static string[] GetDefaulKeys()
            => new string[] { "admin" };

        private static string ReadAsJson(string[] args)
        {
            string input = "";
            foreach (string arg in args)
            {
                input += arg + " ";
            }

            return input;
        }

        static void Help()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine(" Аргументы (JSON): ");
            Console.WriteLine(" \t 1-дочерний процесс");
            Console.WriteLine(" \t 2-имя пользователя");
            Console.WriteLine(" \t 3-пароль");
            Console.WriteLine("\n\n");
            Console.WriteLine(" Пример:");
            Console.WriteLine( new
            {
                process = @"powershell",
                username = "Учитель",
                password = ""
            }.ToJsonOnScreen());
            Console.WriteLine("\n\n:");

        }

        static System.Collections.Generic.IDictionary<string, string> ParseJson(string json)
        {
            try
            {
                Console.WriteLine(json);
                return DeserializeObject<System.Collections.Generic.IDictionary<string, string>>(json);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Не удалось разобрать JSON");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static T DeserializeObject<T>(string json)
        {
            return json.FromJson<T>();
        }

        private string process { get; }
        private string username { get; }
        private string password { get; }
        private string buffer { get; set; } = "";
        private bool waiting { get; set; } = true;

        private IDictionary<string,    
                Func<  
                    IDictionary<string, string>, 
                    IDictionary<string, string>>  > actions;


        private Program(string process, string username, string password)
        {
            this.process = process;
            this.username = username;
            this.password = password;
            this.actions = new Dictionary<string,
                Func<
                    IDictionary<string, string>,
                    IDictionary<string, string>>>();
        }

        private void RunAsConsoleApplication( )
        {
            Console.WriteLine(this.process);
            var process = System.Diagnostics.Process.Start(
            new System.Diagnostics.ProcessStartInfo(
                @"powershell.exe", @"/C "+this.process)
            {
                UseShellExecute = false,
                UserName =              ""+this.username,
                PasswordInClearText =   this.password,
                RedirectStandardError = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            });
            process.Start();
 

            int i = 0;       
            string readed = "";
            while(process.HasExited == false)
            {
                    
                Console.WriteLine(readed = process.StandardOutput.ReadLine());
                Console.WriteLine(readed);

            }
              
         
            process.WaitForExit();
        }


      
        /// <summary>
        /// Выполнение процессачерез коммандную утилиту 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public IDictionary<string,string> Execute(IDictionary<string, string> Request)
        {
            if(Request.ContainsKey("ActionName")==false)
            {
                throw new Exception("Запрос должен содержать параметр ActionName");
            }
            if (actions.ContainsKey(Request["ActionName"]))
            {
                var todo = this.actions[Request["ActionName"]];
                var result = todo(Request);
                return result;
            }
            else
            {
                throw new ArgumentException($"Операция {Request["ActionName"]} не зарегистрирована");
            }
        }

        public IDictionary<string,string> ParseRequest()
        {
            //TODO: onContinueReading
            var requestText = this.buffer.Replace("\r\n", "");
            return ParseJson(requestText);
        }

     

        public void ContinueRead( )
        {
            //TODO: onContinueReading
            //Console.WriteLine(".");
        }


        private static IDictionary<string, string> InteractiveInputParams()
        {
            return InteractiveInputParams("process");
        }


        /// <summary>
        /// Запрос ввода парааметров
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        private static IDictionary<string, string> InteractiveInputParams(params string[] keys)
        {
            var result = new Dictionary<string, string>();
            foreach(var key in  keys)
            {
                result[key] = InputText(key);
            }
            return result;
        }

        private static string InputText(string key)
        {
            Console.WriteLine(key);
            return Console.ReadLine();
        }

        static void OnStart(System.Collections.Generic.IDictionary<string,string> pars, IEnumerable<string> keys){
            foreach (var key in keys)
            {
                switch (key)
                {
                    case "admin":
                        pars["username"] = "Константин";
                        pars["password"] = "sgdf1423";
                        break;
                    default:
                        Console.WriteLine("Недопустимый ключ: "+key);
                        return;

                }
            }
            bool isValid = true;
            if(pars.ContainsKey("process") == false)
            {
                isValid = false;
                Console.WriteLine("В параметрах необходимо указать имя дочернего процесса как process=... ");
            }
            if(pars.ContainsKey("username") == false)
            {
                isValid = false;
                Console.WriteLine("В параметрах необходимо указать имя пользователя как username=... ");
            }
            if(pars.ContainsKey("password") == false)
            {
                isValid = false;
                Console.WriteLine("В параметрах необходимо указать пароль как password=... ");
            }
            if(isValid){
                var program = new Program(pars["process"], pars["username"], pars["password"]);
                
                
                program.RunAsConsoleApplication();                
            }
        }



    }
}
