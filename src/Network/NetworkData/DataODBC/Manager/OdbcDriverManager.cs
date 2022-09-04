using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataODBC;

using Microsoft.AspNetCore.Mvc;




/// <summary>
/// О драйвере
/// </summary>
public class OdbcDriver
{

    public string Name { get; set; }
    public string Platform { get; set; }
    public string Attribute { get; set; }
}



/// <summary>
/// О источнике
/// </summary>
public class OdbcDatasourceDescription
{

    public string Name { get; set; }
    public string DriverName { get; set; }
    public string DsnType { get; set; }
    public string Platform { get; set; }
    public string Attribute { get; set; }

}

 


/// <summary>
/// Оболочка ODBCAD32
/// </summary>
[Controller]

public class OdbcDriverManager: BaseService<OdbcDriverManager>
{



    /// <summary>
    /// Список наименований источников ODBC
    /// </summary>
    /// <returns></returns>
    public string[] GetOdbcDatasourcesNames() => GetOdbcDatasources().Select(d => d.Name).ToArray();


    /// <summary>
    /// Перечень источников данных
    /// </summary> 
    public OdbcDatasourceDescription[] GetOdbcDatasources()
    {
        bool next = true;
        OdbcDatasourceDescription ds = null;
        var dss = new List<OdbcDatasourceDescription>();
        this.Execute("Get-OdbcDsn").ReplaceAll("\r", "").Split("\n").ToList().ForEach(line=>{
            if(string.IsNullOrEmpty(line)){
                if(ds != null)
                    dss.Add(ds);
                next = true;
                
            }
            else
            {
                //System.Console.WriteLine(line.Length+ " "+line);                
                if(next || ds==null)
                    ds = new OdbcDatasourceDescription();
                next = false;
                string[] arr = line.Split(":");
                string key = arr[0].Trim();
                string value = arr[1].Trim();

                //Info(key+"="+value);
                ds.GetType().GetProperty(key).SetValue(ds,value);
            }
        });
        return dss.ToArray();
    }



    /// <summary>
    /// Получения сведений о драйверах ODBC, зарегистрированных в системе
    /// </summary>    
    public OdbcDriver[] GetOdbcDrivers()
    {
        bool next = true;
        OdbcDriver driver = null;
        var drivers = new List<OdbcDriver>();
        this.Execute("Get-OdbcDriver").ReplaceAll("\r", "").Split("\n").ToList().ForEach(line=>{
            if(string.IsNullOrEmpty(line)){
                if(driver != null)
                    drivers.Add(driver);
                next = true;
                
            }
            else
            {
                //System.Console.WriteLine(line.Length+ " "+line);                
                if(next || driver==null)
                    driver = new OdbcDriver();
                next = false;
                string[] arr = line.Split(":");
                string key = arr[0].Trim();
                string value = arr[1].Trim();

                //Info(key+"="+value);
                driver.GetType().GetProperty(key).SetValue(driver,value);
            }
        });
        return drivers.ToArray();
    }




    /// <summary>
    /// Перенаправление управления  в PS
    /// </summary>

    private string Execute(string command)
    {
        System.Console.WriteLine("Выполнение команды: "+command);

        ProcessStartInfo info = new ProcessStartInfo("PowerShell.exe", "/C " + command);
        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
                    
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        System.IO.StreamReader reader = process.StandardOutput;

        string result = reader.ReadToEnd();

        System.Console.WriteLine("Результат выполнения: \n"+result);
        return result;
    }
}