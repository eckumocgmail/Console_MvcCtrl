using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
 


/**
 * 
 * 
 */
public class TypeServiceProvider
{

    public IServiceProvider _serviceProvider;
    public List<ServiceDescriptor> _disc { get; set; }
    public List<string> _services { get; set; }
    public string[] GetServices() => _services.ToArray();
    public object Instance { get; set; }



    public TypeServiceProvider(IServiceProvider serviceProvider)
    {
        Instance = this;
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
}