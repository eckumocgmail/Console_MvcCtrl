using ApplicationCore.Converter.Models;
using ApplicationCore.Domain;

using CommonModule;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonHttp.CommandLine
{

    public class MvcApplicationModel : MyApplicationModel { }
    public class CommandLineAssembly: MvcApplicationModel
    {
        private readonly Assembly _assembly;

        public static CommandLineAssembly Executing { get; set; }

        public CommandLineAssembly(Assembly assembly)
        {
            _assembly = assembly;
        }

        public static void Run(string wrk)
        {
            Console.WriteLine(wrk);
            var cmd = new CommandLineAssembly(Assembly.GetExecutingAssembly());
            cmd.InitExecutable();
            foreach (var ch in cmd)
            {
                ch.ToJsonOnScreen().WriteToConsole();
            }
        }



        /// <summary>
        /// Получение данных о контроллерах реализованных в данной сборке
        /// </summary>
        /// <returns> карта контроллеров </returns>
        public void InitExecutable()
        {
            Info($"InitExecutable()");
            var controllers = GetControllers();
            if (controllers == null || controllers.Count == 0)
            {
                throw new Exception("Контроллеры не найдены в приложении");
            }
            MvcApplicationModel controllersMap = new MvcApplicationModel();
            foreach (Type controllerType in controllers)
            {
                if (controllerType.IsAbstract) continue;

                var model = CreateServiceModel(controllerType);

                this[controllerType.Name] = model;
            }
            Console.WriteLine(controllersMap);            
        }

        private void Info(object v)
        {
            Console.WriteLine(v);
        }

        private List<Type> GetControllers()
        {
            return _assembly.GetTypes().Where(t => t.Name.EndsWith("Controller") && t.IsAbstract == false).ToList();
        }
        public MyControllerModel CreateModel(Type type)
        {
            
            return CreateModel(TypeForShortName(type.GetTypeName()));
        }

        internal void InitChannels()
        {
            throw new NotImplementedException();
        }

        public MyControllerModel CreateModel(string type)
        {
            return CreateModel(TypeForShortName(type));
        }

        private string TypeForShortName(string type)
        {
            return type.ToType().Name;
        }



        public MyControllerModel CreateServiceModel(Type controllerType)
        {
            var uri = "/";
            var attrs = ForType(controllerType);
            if (attrs.ContainsKey("AreaAttribute")) uri += attrs["AreaAttribute"].ToString() + "/";
            if (attrs.ContainsKey("ForRoleAttribute")) uri += attrs["ForRoleAttribute"].ToString() + "/";
            string path = PathForController(controllerType);
            
            MyControllerModel model = new MyControllerModel()
            {
                Name = controllerType.Name.Replace("`1", ""),
                Path = path,
                Actions = new Dictionary<string, MyActionModel>()
            };



            while (controllerType != null)
            {
                foreach (string name in controllerType.GetOwnPublicMethods())
                {
                    var method = controllerType.GetMethods().Where(m=>m.Name==name).FirstOrDefault();
                    if (method.IsPublic && method.Name.StartsWith("get_") == false && method.Name.StartsWith("set_") == false)
                    {

                        IDictionary<string, string> attributes = ForMethod(controllerType, method.Name);
                        IDictionary<string, object> pars = new Dictionary<string, object>();
                        model.Actions[method.Name] = new MyActionModel()
                        {
                            Name = method.Name,
                            Attributes = attributes,
                            Method = ParseHttpMethod(attributes),
                            Parameters = new Dictionary<string, MyParameterDeclarationModel>(),
                            Path = model.Path + "/" + method.Name
                        };
                        foreach (ParameterInfo par in method.GetParameters())
                        {
                            model.Actions[method.Name].Parameters[par.Name] = new MyParameterDeclarationModel()
                            {
                                Name = par.Name,
                                ParameterType = par.ParameterType.Name,
                                ParameterIsOptional = par.IsOptional
                            };
                        }
                    }
                }
                controllerType = controllerType.BaseType;
            }
            return model;
        }

        public static CommandLineAssembly GetExecuting()
        {
            if(Executing == null)
            {
                
                Executing = new CommandLineAssembly(Assembly.GetExecutingAssembly());
              
            }
            return Executing;
            
        }

        private IDictionary<string, string> ForMethod(Type controllerType, string name)
        {
            return COM.AttrUtils.ForMethod(controllerType, name);
        }

        private string ParseHttpMethod(IDictionary<string, string> attributes)
        {
            return COM.AttrUtils.ParseHttpMethod(attributes);
        }

        public static IDictionary<string,string> ForType(Type controllerType)
        {
            return COM.AttrUtils.ForType(controllerType);
        }

        /// <summary>
        /// Метод получения публичных методов типа
        /// </summary>
        /// <param name="type"> тип </param>
        /// <returns> открытые методы </returns>
        private string GetPublicMethods(Type type)
        {
            string actions = "";
            foreach (string name in type.GetOwnPublicMethods())
            {
                var m = type.GetPublicMethod(name);
                var method = m;
                string path = "/" + type.Name;
                actions += "\n\t" +
                    m.Name + "(" + GetMethodParametersString(method) + "){ \n\t\t\t" +
                        "return this.http.get('" + path + "/" + method.Name + "',{params:" +
                            GetMethodParametersBlock(method) + "});\n\t\t\t}\n";
                /* new {
                 name = method.Name,
                 returns= method.ReturnType.Name,
                 args = ReflectionService.GetMethodParameters(method),
                 func = method.Name+"("+ ReflectionService.GetMethodParametersString(method) + 
                     "){ return this.http.get('" + path + "/"+method.Name+"',{params:"+
                         ReflectionService.GetMethodParametersBlock(method) + "});}",

                });*/
            }
            return actions;
        }



        public string GetMethodParametersBlock(MethodInfo method)
        {
            string s = "{";
            bool needTrim = false;
            foreach (var pair in GetMethodParameters(method))
            {
                needTrim = true;
                s += pair.Key + ':' + pair.Key + ",";
            }
            if (needTrim == true)
                return s.Substring(0, s.Length - 1) + "}";
            else
            {
                return s + "}";
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetMethodParameters(MethodInfo method)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            foreach (ParameterInfo pinfo in method.GetParameters())
            {
                args[pinfo.Name] = new
                {
                    type = pinfo.ParameterType.Name,
                    optional = pinfo.IsOptional,
                    name = pinfo.Name
                };
            }
            return args;
        }


        public string GetMethodParametersString(MethodInfo method)
        {
            bool needTrim = false;
            string s = "";
            foreach (var p in GetMethodParameters(method))
            {
                needTrim = true;
                s += p.Key + ",";// +":"+ p.Value + ",";
            }
            return needTrim == true ? s.Substring(0, s.Length - 1) : s;
        }

      

        /// <summary>
        /// <button>ok</button>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public   List<MethodInfo> GetPublicApiMethods(Type type)
        {
           
            return (from m in new List<MethodInfo>(type.GetMethods())
                    where m.IsPublic &&
                            !m.IsStatic &&
                            m.DeclaringType.FullName == type.FullName
                    select m).ToList<MethodInfo>();
        }






        public static string GetPermissionsFor(Type type)
        {
            var attrs = ForType(type);
            if (attrs.ContainsKey("ForRoleAttribute"))
            {
                return attrs["ForRoleAttribute"];
            }
            else
            {
                return null;
            }
        }


        private string PathForController(Type controllerType)
        {
            return "";
            /*string role = RoleFor(controllerType);
            if (role != null)
            {
                return "/" + role + "/" + controllerType.Name.Replace("Controller", "");
            }
            else
            {
                return "/" + controllerType.Name.Replace("Controller", "");
            }*/
        }
    }
}
