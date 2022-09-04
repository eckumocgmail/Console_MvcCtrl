using eckumoc.Services;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eckumoc_netcore_cmd_builder.ConsoleCmdBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public class CmdBuilder: StringWriter
    {
        private readonly string _dir;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        public CmdBuilder(string dir=null)
        {
            if (dir == null)
                dir = System.IO.Directory.GetCurrentDirectory();
            _dir = dir;
        }

        public void Init(string dir)
        {
            string json = Trace();
            
            if (Environment.UserInteractive)
            {
                Console.WriteLine(_dir);
                Console.WriteLine("\n\n");
                Console.WriteLine(json);
                Console.WriteLine("\n\n");
                System.IO.File.WriteAllText(_dir + @"\\api.json", json);
                Console.WriteLine("Продолжаем (y/n), e.t.c по-умолчанию продолжаем");
                if (Console.ReadLine().ToLower() == "n")
                {
                    throw new Exception();
                }

                
            }
            else
            {
                System.IO.File.WriteAllText(_dir + @"\\api.json", json);
            }
            System.IO.Directory.GetDirectories(_dir).ToList().ForEach(Init);
        }

 
        /// <summary>
        /// 
        /// </summary>
        public void Build()
        {
            string json = Trace();
            if(Environment.UserInteractive)
            {
                Console.WriteLine(_dir);
                Console.WriteLine("\n\n");
                Console.WriteLine(json);
                Console.WriteLine("\n\n");
                Console.WriteLine("Продолжаем (y/n), e.t.c по-умолчанию продолжаем");
                if(Console.ReadLine().ToLower()=="n")
                {
                    throw new Exception();
                }


            }
            else
            {
                System.IO.File.WriteAllText(_dir + @"\\api.json", json);
            }            
        }


        /// <summary>
        /// ФОрматирует текст комманды
        /// </summary>        
        public string Trace()
        {
            
            foreach (var dll in System.IO.Directory.GetFiles(_dir, "*.dll"))
            {

                string config = JsonConvert.SerializeObject(
                    DllExecuter.CliConfiguration("controller.exe", dll),
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented
                    }
                );
                WriteLine(config);
            }
            return this.ToString();
        }

    
    }
}
