using System.Collections.Generic;

namespace eckumoc.Services
{
    public interface IDll
    {
        Dictionary<string, object> CliConfiguration(string program, string filename);
        string Execute(string dllfilename, string classname, string method, string args);
        string toSnakeStyle(string[] ids);
    }
}