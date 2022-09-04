using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataADO;

using RootLaunch;

public class FileResource:  TestingElement
{
    private DateTime AccessTime;
    private DateTime WriteTime;

   

    public string Path { get; }
 
    public bool IsDirectory { get => System.IO.Directory.Exists(Path); }
    public bool IsFile { get => System.IO.File.Exists(Path); }
    public string NameShort {
        get => GetNameShort(); 
    }
    public string GetNameShort()
    {
        return this.Path.Substring(ResourceManager.GetParentPath(this.Path).Length + 1);
    }
    public string GetName()
    {
        int i=NameShort.IndexOf(".");
        return i==-1? NameShort: NameShort.Substring(0, i);
    }
    public string GetExt()
    {        
        var shName = NameShort;
        var i = shName.IndexOf(".");

        return shName.Substring(i + 1);        
    }
    
    private byte[] Data { get; set; }
    public bool IsInner { get; }

    public byte[] ReadBytes()
    {
        if (Data == null)
        {
            this.Data = System.IO.File.ReadAllBytes(this.Path);
        }
        return this.Data;
    }

    public FileResource()
    {
    }

    public FileResource( string pathAbs )
    {                        
        this.Path = pathAbs;
        this.ID = this.NameShort.GetHashCode();
 

        if (this.IsDirectory==false && this.IsFile == false)
        {
            this.IsInner = this.Path.StartsWith(System.IO.Directory.GetCurrentDirectory());
            System.IO.File.Create(this.Path);
            if (this.IsDirectory == false && this.IsFile == false )
            {
                throw new Exception($"[404][" + pathAbs + $"] => Путь=[{pathAbs}] не существует такого файла, кстати говоря я проверил директории что такой нету.. .");
            }
        }
        
    }

    public object Modify(string v)
    {
        throw new NotImplementedException();
    }

    public virtual void OnInit()
    {         
        this.AccessTime = System.IO.File.GetLastAccessTimeUtc(this.Path);
        this.WriteTime = System.IO.File.GetLastWriteTime(this.Path);                    
    }


    public byte[] GetBInaryData() => this.Data;



    public virtual bool Copy(string directory)
    {
        var ctrl = DirectoryResource.Get(directory);
        ctrl.CreateFile(NameShort).WriteText(ReadText());
        return true;
    }

    public string ReadText()
    {
        this.Info($"ReadText({this.NameShort})");
        return System.IO.File.ReadAllText(this.Path);
    }
    public void WriteText(string context)
    {
        //this.Info($"WriteText({this.Path})");
        System.IO.File.WriteAllText(this.Path, context);
    }
    public async Task<string> ReadTextAsync()
    {
        return await System.IO.File.ReadAllTextAsync(this.Path);
    }
    public async Task WriteTextAsync(string context)
    {
        await System.IO.File.WriteAllTextAsync(this.Path, context);
    }

    public virtual DirectoryResource[] GetDirectories()
    {
        /*if (IsFile)
        {
            throw new Exception("Не возможно получить список директорий для файла " + this.Path);
        }*/
        try{
            var dirs = System.IO.Directory.GetDirectories(this.Path);
            return dirs.Select(path => DirectoryResource.Get(path)).ToArray();
        }catch(Exception ex){
            Error(ex);
            
            throw;
        }
        
    }

    public virtual FileResource[] GetFiles()
    {
        if (IsFile)
        {
            throw new Exception("Не возможно получить список директорий для файла " + this.Path);
        }
        return System.IO.Directory.GetFiles(this.Path).Select(path => new FileResource(path)).ToArray();
    }

    public virtual FileResource[] GetAllFiles()
    {
        var resources = new List<FileResource>();
        if (IsFile)
        {
            throw new Exception("Не возможно получить список директорий для файла " + this.Path);
        }
        resources.AddRange(System.IO.Directory.GetFiles(this.Path).Select(path => new FileResource(path) ).ToArray());
        GetDirectories().ToList().ForEach(dir=> resources.AddRange(dir.GetAllFiles()));
        return resources.ToArray();
    }

    private FileResource GetFiles(string path)
    {
        return new FileResource(ResourceManager.GetChildPath(this.Path, path));
    }

    public virtual FileResource GetParent(   )
    {
        return new FileResource(ResourceManager.GetParentPath(this.Path));
    }

    

    public override string ToString()
    {
        return Path;
    }

    protected override void OnTest()
    {
        throw new NotImplementedException();
    }
}
