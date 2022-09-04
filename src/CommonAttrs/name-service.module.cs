using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tools;

public static class CS
{
}

namespace NameSpaceModule
{




    /// <summary>
    /// Доступ к контейнер службы каталогов
    /// </summary>
    public class NameServiceProvider: IServiceProvider
    {



        private readonly IServiceProvider _serviceProvider;
        public List<ServiceDescriptor> _disc { get; set; }
        public List<string> _services { get; set; }



        public string[] GetServices() => _disc.Select(s=>s.ServiceType.Name).ToArray();

        public object Instance { get; set; }

        public NameServiceProvider(IServiceProvider serviceProvider, List<ServiceDescriptor> disc)
        {
            Instance = this;
            _disc = disc;
            _serviceProvider = serviceProvider;
            _services = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.FullName != null && type.FullName.StartsWith("Tools")).Select(type => type.Name).ToList();
        }

        public bool SetService(string id)
        {
            Instance = GetService(id);
            return true;
        }

        public bool AddService(string id)
        {
            _services.Add(id);
            return true;
        }

        public object GetService(string id)
        {
            return _serviceProvider.GetRequiredService(GetType(id));
        }

        public Type GetType(string id)
        {
            return _disc.Where(desc => desc.ServiceType.Name == id).FirstOrDefault().ServiceType;
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }


    public class LocalServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            
            return new PowerShell(serviceType.Namespace.ReplaceAll(".", @"\") + ".exe");
        }
    }
}
