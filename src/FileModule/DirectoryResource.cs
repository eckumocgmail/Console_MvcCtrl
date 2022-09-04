using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootLaunch
{

  
    /// <summary>
    /// 
    /// </summary>
    public class DirectoryResource : FileResource, IDirectoryResource
    {
        public static DirectoryResource Get(string arbsPath=null)
        {
            if (arbsPath == null)
                arbsPath = System.IO.Directory.GetCurrentDirectory();
            if (Directories.ContainsKey(arbsPath)==false)
            {
                Directories[arbsPath] = new DirectoryResource(arbsPath);
                Directories[arbsPath].OnInit();
            }
            return Directories[arbsPath];
        }
        private static IDictionary<string, DirectoryResource> Directories { get; } =
                    new ConcurrentDictionary<string, DirectoryResource>();

         

        private IDictionary<string, DirectoryResource> SubDirs { get; set; } = new ConcurrentDictionary<string, DirectoryResource>();


        /// <summary>
        /// 
        /// </summary>
        public DirectoryResource() : this(System.IO.Directory.GetCurrentDirectory())
        {
        }

        public DirectoryResource(string pathAbs) : base(pathAbs)
        {
            if (Directories.ContainsKey(Path))
            {
                Directories[pathAbs].Error(new Exception(
                    $"Ресурсы директории уже созданы их нужно использовать через " +
                    $"{nameof(DirectoryResource) + "." + nameof(DirectoryResource.Get)}"));                
            }
            else
            {
                Directories[pathAbs] = this;
            }
            
        }

        /// <summary>
        /// Устанавливает собственный путь в качестве рабочей директории
        /// </summary>
        public void SetCurrentDirector()
        {            
            System.IO.Directory.SetCurrentDirectory(this.Path);
        }


        public FileResource[] GetExeFiles()
            => GetFiles().Where(f => f.NameShort.EndsWith(".exe")).ToArray();
        public FileResource[] GetDllFiles()
            => GetFiles().Where(f => f.NameShort.EndsWith(".dll")).ToArray();


        public override void OnInit()
        {
            this.GetDirectories().ToList().ForEach(OnInitSubDir);
        }

        private void OnInitSubDir(DirectoryResource dir)
        {
            Directories[dir.Path] =
                this.SubDirs[dir.NameShort] = dir;
        }


        public void Trace()
        {
            System.IO.Directory.GetDirectories(this.Path).ToJsonOnScreen().WriteToConsole();
            System.IO.Directory.GetFiles(this.Path).ToJsonOnScreen().WriteToConsole();

        }

        public string GetDirectoryName() => this.Path.Substring(this.Path.LastIndexOf(ResourceManager.GetPathSeparator()) + 1);

        public FileController<T> Bind<T>(string filename) where T : class
        {
            string filePath = this.Path + ResourceManager.GetPathSeparator() + filename;
            return new FileController<T>(filePath);
        }
        public FileResource CreateFile(string filename)
        {
            string dirpath = this.Path + ResourceManager.GetPathSeparator() + filename;
            System.IO.File.WriteAllText(dirpath, "");
            return new FileResource(dirpath);
        }
        public FileResource CreateTextFile(string filename, string text)
        {
            string dirpath = this.Path + ResourceManager.GetPathSeparator() + filename;
            System.IO.File.WriteAllText(dirpath, "");
            var ctrl = new FileResource(dirpath);
            ctrl.WriteText(text);
            return ctrl;
        }
        public DirectoryResource GetOrCreateDirectory(string dirname)
        {
            string dirpath = this.Path + ResourceManager.GetPathSeparator() + dirname;
            if (System.IO.Directory.Exists(dirpath) == false)
            {
                System.IO.Directory.CreateDirectory(dirpath);
            }
            return DirectoryResource.Get(dirpath);
        }
        public override bool Copy(string directory)
        {
            var ctrl =   DirectoryResource.Get(directory);

            foreach (var file in GetFiles())
            {
                ctrl.CreateTextFile(file.NameShort, file.ReadText());
            }
            foreach (var dir in GetDirectories())
            {
                var subctrl = ctrl.GetOrCreateDirectory(dir.NameShort);
                subctrl.Copy(directory + ResourceManager.GetPathSeparator() + dir.NameShort);
            }
            return true;
        }

        public override string ToString()
        {
            return NameShort;
        }
    }
}
