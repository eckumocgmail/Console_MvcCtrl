namespace RootLaunch
{
    public interface IDirectoryResource
    {
        string GetName();


        FileController<T> Bind<T>(string filename) where T : class;


        bool Copy(string directory);

        FileResource CreateFile(string filename);
        FileResource CreateTextFile(string filename, string text);
        FileResource[] GetDllFiles();
        FileResource[] GetExeFiles();
        
        DirectoryResource GetOrCreateDirectory(string dirname);
       
        void SetCurrentDirector();       
        void Trace();
    }
}