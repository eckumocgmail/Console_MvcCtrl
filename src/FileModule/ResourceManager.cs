using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class ResourceManager
{
    public static Dictionary<string, object> DEPS;
    public static string PATH_SEPARATOR = null;

    /**
        * Получение карты ресурсов типа изображение
        */
    public static Dictionary<string,string> GetFileIcons( )
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach ( string filename in GetAppDataResources( "resources\\images\\icons" ) )
        {
            result[filename] = "api/App?path=resource.ReadFile&pars={path:\"" + filename + "\"}";
        }
        return result;
    }

    /**
        * Получение карты ресурсов типа изображение
        */
    public static Dictionary<string, string> GetFileIconsFromDir( string dir)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach ( string filename in GetAppDataResources( dir ) )
        {
            result[filename] = "api/App?path=resource.ReadFile&pars={path:\"" + filename + "\"}";
        }
        return result;
    }


    public static string GetParentPath(string path)
    {
        string sep = GetPathSeparator();
        int ind = path.LastIndexOf(sep);
        if (ind == -1)
        {
            throw new Exception("Не возможно получить путь к родительской директории для "+path);
        }
        else
        {
            return path.Substring(0,ind);
        }        
    }

    /**
     * возвращает файловый разделитель 
     */
    public static string GetPathSeparator( )
    {
        if( PATH_SEPARATOR ==null )
        {
            string path = GetRootDirectory();
            int i0 = path.LastIndexOf( "/" );
            int i1 = path.LastIndexOf( "\\" );
            if ( i0 > i1 )
            {
                PATH_SEPARATOR = "/";
            }
            else if ( i1 > i0 )
            {
                PATH_SEPARATOR = "\\";
            }
            else
            {
                throw new Exception("ResourceManager: can not compute file path separator");
            }
        }
        return PATH_SEPARATOR;
    }

    public static void TraceAppData()
    {
        ResourceManager.GetAppData().ToList().ForEach((kv) =>
        {
            System.Console.WriteLine(kv.Key + "=" + kv.Value);
        });
    }

    /**
        * Получение всех ресурсов каталога AppData
        */
    public static Dictionary<string, object> GetAppData( )
    {
        return GetAppDataForDirectory(GetRootDirectory());
    }

    /**
        * Получение всех ресурсов каталога AppData
        */
    public static Dictionary<string, object> GetAppDataForDirectory( string path )
    {
        Dictionary<string, object> resources = new Dictionary<string, object>();
        foreach( string dir in System.IO.Directory.GetDirectories( path ) )
        {
            string name = dir.Substring( dir.LastIndexOf( GetPathSeparator() ) + 1 );
            resources[name] = GetAppDataForDirectory( dir );
        }
        foreach ( string file in System.IO.Directory.GetFiles( path ) )
        {
            string name = file.Substring( file.LastIndexOf( GetPathSeparator() ) + 1 );
            resources[name] = System.IO.File.ReadAllBytes( file );
        }
        return resources;
    }

    /**
        * возвращает абсолютный путь к корневой директории проекта
        */
    public static string GetRootDirectory( )
    {
        return System.IO.Directory.GetCurrentDirectory();
    }

    /**
        * возвращает абсолютный путь директории статических ресурсов проекта
        */
    public static string GetAppDataDirectory( )
    {
        return System.IO.Directory.GetCurrentDirectory() + @"\AppData";
    }

    /**
        * Возвращает список файлов из заданного подкаталога AppData
        */
    public static string[] GetAppDataResources( string dir )
    {
        List<string> files = new List<string>( System.IO.Directory.GetFiles( GetAppDataDirectory() + @"\" + dir ) );
        files.Sort();
        return files.ToArray();
    }

    public static string GetChildPath(string path1, string path2)
    {
        return $"{path1}{GetPathSeparator()}{path2}";
    }

    /**
        * Возвращает ресурсы для клиентского приложения
        */
    public static Dictionary<string, object> GetClientAppDeps( )
    {
        if ( DEPS == null )
        {
            DEPS = new Dictionary<string, object>();
            
            foreach ( string file in GetAppDataResources( "client" ) )
            {
                string data = System.IO.File.ReadAllText( file ); ;   
                DEPS[file] = data;
            }
        }
        return DEPS;
    }

    /**
        * получение списка ресурсов
        
    public static string[] ResourceList( )
    {
        List<string> names = new List<string>();
        foreach(JObject next in WebApp.GetDataSource().Execute("select resource_name from resources"))
        {
            names.Add(next["resource_name"].Value<string>());
        }
        return names.ToArray();
    }

    /**
        * выгрузка бинарных данных из хранилище
        * /
    public static byte[] Download(string id)
    {
        return WebApp.GetDataSource().ReadBlob("select resource_data from resources where resource_id=" + id);
    }

    /**
        * загрузка бинарных данных в хранилище
        * /
    public static void Upload(string name, string mime, byte[] data)
    {
        WebApp.GetDataSource().InsertBlob("insert into resources (resource_name,mime_type,resource_data) values (\"" + name + "\",\"" + mime + "\",?)", "@bin_data", data);
    }*/

        

}
