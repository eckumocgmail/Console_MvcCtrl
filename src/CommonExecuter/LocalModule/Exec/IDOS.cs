using System.Collections.Generic;

namespace Tools

{
    public interface IDOS
    {
        string Execute(string command);
    }
}
public interface ICMD
{
    string CmdExec(string command);
    string Find(string regularExpression, string filename);
    Dictionary<string, object> Search(string regularExpression);
    string Search(string path, string pattern);
    void StartProcess(string command);
}