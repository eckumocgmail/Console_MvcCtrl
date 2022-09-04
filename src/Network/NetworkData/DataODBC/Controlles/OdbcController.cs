using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using ApplicationCore.Converter.Models;

using COM;

using DataODBC;


using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class OdbcController : Controller
{
    /// <summary>
    /// Примитивные типы данных
    /// </summary>
    private HashSet<string> PrimitiveTypeNames = new HashSet<string>() {
        "String", "Boolean", "Double", "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64" };



    public OdbcController()
    {

    }

    public object Index()
    {


        return new OdbcDriverManager().GetOdbcDatasourcesNames();
    }



    [HttpGet("/api/[controller]")]
    public Task<string> Options() {
        return Task.Run(() => {
            return "adasdsa";
        });
    }






    //GET #"/Odbc&{datasourceName}&{entity}&{operation}&args={args}"
    [CanGet("[controller]")]
    
    public async Task<string> Get(
        
        string datasourceName,       
        string entity,            
        string operation, 
        string args)
    {

        

        
            
          
        //Выполнение операции            
        try
        {
            //проверка параметра operation

            if (new OdbcDriverManager().GetOdbcDatasourcesNames().ToList().Contains(datasourceName) == false)
            {
                return new
                {
                    Status = "Failed",
                    Pattern = "?datasourceName={'Get(\"????\")'}&entity={entity}&operation={'List'|'Keywords'|'Search'|'Page'|'Delete'|'Update'|'Create'}&args={args}",
                    Text = "Источник данных не зарегистрирован",
                    DataSources = new OdbcDriverManager().GetOdbcDatasources(),
                    Drivers = new OdbcDriverManager().GetOdbcDrivers()
                }.ToJsonOnScreen();
            }

            var datasource = OdbcDatabaseManager.GetOdbc(datasourceName);


            if (entity == null)
            {
                return await Task.Run(
                    () => {
                        return datasource.GetDatabaseMetadata().ToJsonOnScreen();
                    }
                );
            }


            //проверка параметра entity

            var manager = datasource;


            if (manager.fasade.ContainsKey(entity) == false)
            {
                return await Task.Run(
                    () => {
                        return manager.fasade.Keys.ToList().ToJsonOnScreen();
                    }
                );
            }
            var fasade = manager.fasade[entity];


            object result = null;
            var dictionary = args!=null? args.FromJson<Dictionary<string,int>>(): null;
            switch (operation)
            {
                case "List":
                    result = fasade.SelectAll();
                    break;
                case "Keywords":
                       
                    string keywordsQuery = dictionary["query"].ToString();                        
                    List<string> keywords = fasade.GetKeywords(entity, keywordsQuery);                        
                    result = new
                    {
                        Query = keywordsQuery,                          
                        Results = keywords
                    };
                    break;
                    
                case "Search":
                    
                    string searchedQuery = dictionary["query"].ToString();
                    int searchedPage = int.Parse(dictionary["page"].ToString());
                    int searchedPageSize = int.Parse(dictionary["size"].ToString());
                    JArray qurable = fasade.Search(entity,searchedQuery);
                    int totalSize = qurable.Count;
                    //qurable = repository.Page(qurable,searchedPage,searchedPageSize);
                    result = new
                    {
                        Page = searchedPage,
                        PageSize = searchedPageSize,
                        TotalResults = totalSize,
                        TotalPages = ((totalSize % searchedPageSize) == 0) ? ((int)(totalSize / searchedPageSize)) : (1 + ((int)((totalSize - ((totalSize % searchedPageSize))) / searchedPageSize))),
                        Results = qurable
                    };
                    break;
                    
                case "Page":
                        
                    int page = int.Parse(dictionary["page"].ToString());
                    int size = int.Parse(dictionary["size"].ToString());
                    long c = fasade.Count();
                    result = new
                    {
                        TotalResults = c,
                        TotalPages = ((c % size) == 0) ? ((int)(c / size)) : (1 + ((int)((c - ((c % size))) / size))),
                        Results = fasade.SelectPage(page, size)
                    }; 
                    break;
                case "Delete":
           
                    int deleteid = int.Parse(dictionary["id"].ToString());
                    fasade.Delete(deleteid);
                    result = 1;
                    break;
                case "Find":
                       
                    int findid = int.Parse(dictionary["id"].ToString());
                    result = fasade.Select(findid); 
                    break;
                case "Update":
                        
                    Dictionary<string, object> pars = Deseriallize(args);
                    object model = pars["model"];
                    fasade.Update(model.ToDictionary());
                    result = 1;
                    break;
                case "Create":
                        
                    Dictionary<string, object> cpars = Deseriallize(args);
                    object cmodel = cpars["model"];
                    fasade.Create(cmodel.ToDictionary());
                    result = 1;
                    break;
                default:
                    return new
                    {
                        Status = "Success",
                        Metadata = manager.GetMetadata().Tables[entity]
                    }.ToJson();

            }
            return new
            {
                Status = "Success",
                
                Result = result
            }.ToJson();
        }
        catch (Exception ex)
        {
            return new
            {
                Status =    "Failed",
                Stack = ex.StackTrace,
                Message =   "Выполнение операции прервано по причине: "+ ex.Message
            }.ToJsonOnScreen();
        }
    }


     
    public bool IsPrimitive(object subject) {
        Type type = subject.GetType();
        return type.IsPrimitive || PrimitiveTypeNames.Contains(subject.GetType().Name);
    }
        
    public object CopyFromDictionary(object target, Dictionary<string,object> valuesMap)
    {
        MyActionModel model = new MyActionModel();
        foreach (string property in GetPropertyNames(target))
        {
            if (valuesMap.ContainsKey(property))
            {
                object value = valuesMap[property];
                string propertyTypeName = model.GetProperty(property).GetType().GetTypeName();
                if (value != null && (value.GetType().Name == "Int64"|| propertyTypeName == "Int32"))
                {
                    value = Int32.Parse(value.ToString());
                }
                    
                target.GetType().GetProperty(property).SetValue(target, value);
            }
        }
        return target;
    }
    public object Copy(object target, object from)
    {
        foreach(string property in GetPropertyNames(target))
        {
            object value = from.GetType().GetProperty(property).GetValue(from);
            target.GetType().GetProperty(property).SetValue(target,value);
        }
        return target;
    }

    public object Create(Type type)
    {
        ConstructorInfo constructor = 
            (from c in new List<ConstructorInfo>(type.GetConstructors()) where c.GetParameters().Length == 0 select c).SingleOrDefault();
        return constructor.Invoke(new object[0]);
    }

    public object FromJson( string json )
    {
        Dictionary<string, object> valuesMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        object model = null;
        if (valuesMap.ContainsKey("type"))
        {
            string type = valuesMap["type"].ToString();
            valuesMap.Remove("type");
            Type modelType = ReflectionService.TypeForName(type);
            model = Create(modelType);
            model = CopyFromDictionary(model, valuesMap);
        }
        else
        {
            model = JsonConvert.DeserializeObject<object>(json);
        }            
        return model;
    }

       

    public Dictionary<string, object> ToValuesMap(object user)
    {
        Dictionary<string, object> valuesMap = new Dictionary<string, object>();
        GetPropertyNames(user).ForEach(name =>
        {
            valuesMap[name] = user.GetType().GetProperty(name).GetValue(user);
        });
        valuesMap["type"] = user.GetType().FullName;
        return valuesMap;
    }

    private List<string> GetPropertyNames(object model)
    {
        List<string> properties = new List<string>();
        string name = model.GetType().Name;
        if( name == "Object" || name == "JObject")
        {
            return properties;
        }
        foreach (var prop in model.GetType().GetProperties())
        {
            properties.Add(prop.Name);
        }
        return properties;
    }

    public void Resolve(object myNavigationOptions, Dictionary<string, string> dic)
    {
        foreach(var p in ToValuesMap(myNavigationOptions))
        {
            dic[p.Key] = p.Value.ToString();
        }

    }

    public object CopyFromDictionarySave(object target, Dictionary<string, object> valuesMap)
    {
        MyActionModel model = new MyActionModel();
        foreach (string property in GetPropertyNames(target))
        {
            if (valuesMap.ContainsKey(property))
            {
                try
                {
                    object value = valuesMap[property];
                    string propertyTypeName = model.GetProperty(property).GetType().GetTypeName();
                    if (value != null && (value.GetType().Name == "Int64" || propertyTypeName == "Int32"))
                    {
                        value = Int32.Parse(value.ToString());
                    }

                    target.GetType().GetProperty(property).SetValue(target, value);
                }
                catch(Exception)
                {
                    continue;
                }
            }
        }
        return target;
    }

    private Dictionary<string, object> Deseriallize(string json)
    {
        Dictionary<string, object> pars = new Dictionary<string, object>();
             
        Dictionary<string, object> parametersMap =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        foreach (string key in parametersMap.Keys)
        {
            if (IsPrimitive(parametersMap[key]) == false)
            {
                string propertyJson = JObject.FromObject(parametersMap[key]).ToString();
                object value = FromJson(propertyJson);

                pars[key] = value;

            }
            else
            {
                if (parametersMap[key].GetType().Name == "Int64")
                {
                    pars[key] = Int32.Parse(parametersMap[key].ToString());
                }
                else
                {
                    pars[key] = parametersMap[key];
                }

            }
        }
        return pars;
    }
}