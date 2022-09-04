using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
 

namespace eckumoc.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class Dll : IDll
    {
        public Dll()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllfilename"></param>
        /// <param name="classname"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Execute(string dllfilename, string classname, string method, string args)
        {
            Assembly dll = Assembly.LoadFile(dllfilename);
            Type type = (from t in new List<Type>(dll.GetTypes()) where t.FullName == classname select t).SingleOrDefault<Type>();
            MethodInfo meth = type.GetMethod(method);
            object res = Invoke(meth, null, JsonConvert.DeserializeObject<JObject>(args));
            return JObject.FromObject(res).ToString();
        }

        private object Invoke(MethodInfo meth, object p, JObject jObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string toSnakeStyle(string[] ids)
        {
            string snaked = "";
            foreach (var id in ids)
            {
                snaked += "_" + id;
            }
            snaked = snaked.Substring(1);
            return snaked;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="program"></param>
        /// <param name="filename"></param>
        /// <returns> commands line prototype </returns>
        public Dictionary<string, object> CliConfiguration(string program, string filename)
        {
            Dictionary<string, object> apps = new Dictionary<string, object>();
            Assembly dll = Assembly.LoadFile(filename);
            foreach (var type in dll.GetTypes())
            {

                string fullname = type.FullName;


                foreach (var mi in type.GetMethods())
                {
                    if (mi.IsPublic && mi.IsStatic)
                    {

                        string cli = toSnakeStyle(fullname.ToLower().Split("."));
                        string help = "";
                        string header = fullname + "\n\t";
                        string execute = $"dotnet {program} /f {filename} /c {fullname} /m {mi.Name} ";

                        foreach (var pi in mi.GetParameters())
                        {
                            help += "\t" + pi.Name + " " + pi.ParameterType.Name + "\n";
                            execute += "--" + pi.Name + " %" + pi.Name + "%";
                        }
                        header += "in automatization mode execute command: " + execute + "\n\t";
                        header += "in user interface mode available params: " + execute + "\n\t";
                        apps[fullname] = new
                        {
                            filename = filename,
                            fullname = fullname,
                            help = (header + help).Split("\n"),
                            cli = cli,
                            execute = execute

                        };

                    }
                }
            }
            return apps;
        }
    }
}
