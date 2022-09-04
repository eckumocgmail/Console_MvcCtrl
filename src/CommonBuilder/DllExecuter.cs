using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using eckumoc_netcore_cmd_builder.ConsoleCmdBuilder;
using static EcKuMoC;
using static ConsumerController;

     
    public class DllExecuterTest: TestingElement
    {
        protected override void OnTest()
        {
            
        }
    }
public class DllExecuter
{



        


    public static int Run(string[] args)
    {
        var cmd = new CMD();
        foreach (var arg in args)
        {
            WriteLine(cmd.CmdExec(arg));
        }
        return 1;
    }

    public static string Execute(string dllfilename, string classname, string method, string args )
    {
        Assembly dll = Assembly.LoadFile(dllfilename);
        Type type = (from t in new List<Type>(dll.GetTypes()) 
                        where t.FullName == classname select t).SingleOrDefault<Type>();
        MethodInfo meth = type.GetMethod(method);
        object res = JsonExecuter.Invoke(meth, null, JsonConvert.DeserializeObject<JObject>(args));
        return JObject.FromObject(res).ToString();
    }

    public static string toSnakeStyle(string[] ids)
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
    public static Dictionary<string, object> CliConfiguration(string program, string filename)
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
                    string execute = $"{program} /f {filename} /c {fullname} /m {mi.Name} ";

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

    public static Dictionary<string, object> Discover(Type type)
    {
        Dictionary<string, object> actionMetadata = new Dictionary<string, object>();
        foreach (MethodInfo info in type.GetMethods())
        {
            if (info.IsPublic && info.IsStatic)
            {
                Dictionary<string, object> args = new Dictionary<string, object>();
                foreach (ParameterInfo pinfo in info.GetParameters())
                {
                    args[pinfo.Name] = new
                    {
                        type = pinfo.ParameterType.Name,
                        optional = pinfo.IsOptional,
                        name = pinfo.Name
                    };
                }
            }
        }
        return actionMetadata;
    }
}