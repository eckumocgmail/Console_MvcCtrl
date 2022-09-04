 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace eckumoc.Services
{
    public class AssemblyParser
    {
        public static Assembly CreateAssembly( string filename )
        {
            return Assembly.LoadFile(filename);
        }

        public static HashSet<Type> GetControllers( string filename )
        {
            Assembly assembly = CreateAssembly(filename);
            return GetControllers(assembly);
        }

        public static HashSet<Type> GetControllers( Assembly assembly )
        {
            return GetTypeExtensions(assembly, "ControllerBase");
        }

        public static HashSet<Type> GetHubs(Assembly assembly)
        {
            return GetTypeExtensions(assembly, "Hub");
        }
        private IEnumerable<char> ForPublicMethods(Type type, Func<MethodInfo, string> ForMethod)
        {
            return type.GetOwnStaticMethods().ToList().SelectMany(name => "\n"+ForMethod(type.GetMethod(name)));
        }
        public static HashSet<Type> GetTypeExtensions(Assembly assembly, string baseType )
        {
            HashSet<Type> types = new HashSet<Type>();
            Type typeOfObject = new object().GetType();           
            foreach (Type type in assembly.GetTypes())
            {
                Type p = type.BaseType;
                while (p != typeOfObject)
                {
                    if (p.Name == baseType)
                    {
                        types.Add(type);
                        break;
                    }
                    p = p.BaseType;
                }
            }
            return types;
        }
    }
}
