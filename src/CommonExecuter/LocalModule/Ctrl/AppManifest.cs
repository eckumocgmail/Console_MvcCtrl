using Newtonsoft.Json;

using System;

public partial class AppManifest 
{
    public string Name { get; set; } = "Noname";
    public string Description { get; set; } = "";
    public string Version { get; set; } = "1.0.0.0.0";
    public int Build { get; set; } = 0;
    public int Port { get; set; } = 0;
    public string Author { get; set; } = "eckumoc@gmail.com";
    public string Location { get; set; }
    public string Updated { get; set; } = DateTime.Now.ToString();



    public string GetHomePage() => 
        $"http://localhost:{Port}/Home/Index";
        //$"http://localhost:{Port}/{Name.ReplaceAll(".","_")}/Index";
    
}

 