using eckumoc.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace eckumoc_netcore_json
{    
    public class ConsoleJsonProgram
    {
        public static void Run(string[] args)
        {
            OnStart(args);
        }

        public static void OnStart(string[] args)
        {

           
            if (args.Length == 0)
            {
                WriteHelp();
            }
            else
            {
                string result = "{ \"message\": \"wrong parameters\" }";
                switch (args[0].ToLower())
                {
                    case "names":
                        result = "";
                        List<string> keys = new List<string>();
                        Dictionary<string, object> property = JsonConvert.DeserializeObject<Dictionary<string,object>>(JsonFileEditor.Get(args[1], args[2]).ToString());
                        foreach(var p in property)
                        {
                            result += "\n" + p.Key;
                            keys.Add(p.Key);
                        }
                        if (result.Length > 0) result = result.Substring(1);
                        break;
                    case "get":
                        result = JsonFileEditor.Get(args[1], args[2]).ToString();
                        break;
                    case "set":
                        JsonFileEditor.Set(args[1], args[2], args[3]) ;
                        result = "{ \"message\": \"success\" }";
                        break;
                    default: break;

                }
                Console.WriteLine(result);
            }
             
        }

        private static void WriteHelp()
        {
            Console.WriteLine("edit jsonfiles");
            Console.WriteLine("usage:");
            Console.WriteLine("  json [file-path] names|get|set [parameters]");
            Console.WriteLine("example:");
            Console.WriteLine("  json appsettings.json names");            

        }

        
    }
}
 