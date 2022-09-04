using ApplicationCore.Converter.Models;

using Newtonsoft.Json;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Converter.Models
{
    
}


public class MyControllerModel
{
    public string Name { get; set; }
    public string Path { get; set; }


    public IDictionary<string, MyActionModel> Actions { get; set; } = new Dictionary<string, MyActionModel>();


    /// <summary>
    /// Запись информации о методах в справочник
    /// </summary>
    /// <param name="data"></param>
    public void WriteTo(IDictionary data)
    {
        Actions.ToList().ForEach(a =>
        {
            data[a.Key] = JsonConvert.SerializeObject(a.Value);
        });
    }


    public string GetAnnotationForService()
    {
        return "@Injectable({ providedIn: 'root' })\n";
    }


    public string GetImportsForService()
    {
        return
            "import { Observable } from 'rxjs';\n" +
            "import { Injectable } from '@angular/core';\n" +
            "import { HttpClient } from '@angular/common/http';\n\n";
    }
}