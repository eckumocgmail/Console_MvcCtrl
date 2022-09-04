using System;
using System.Collections.Generic;
using System.Threading;
using Tools;


public interface IAspSource
{
    public ISet<string> imports { get; set; }
    public IList<string> lines { get; set; }
}






public interface IAspSourcePackageManager
{

}

public class AspSourcePackageManager: IAspSourcePackageManager
{

}



namespace CsNamespaceHelper
{
    class Program
    {


        //пути ко всем исходникам в директории
        public static List<string> GetCsSrcFiles(string dir)
        {
            List<string> files = new List<string>();
            foreach(string path in System.IO.Directory.GetFiles(dir))
            {
                if(path.EndsWith(".cs")){
                    files.Add(path);
                }
            }
            foreach(string subdir in System.IO.Directory.GetDirectories(dir))
            {
                files.AddRange(GetCsSrcFiles(subdir));
            }
            return files;
        }



        //пути ко всем исходникам в директории
        public static void PutToNamespae(string ns, string dir, string output)
        {
            string tab = "  ";
            string src = "";
            bool importSection = true;
            var lines = new List<string>();
            string declarations = "";
            foreach(string file in GetCsSrcFiles(dir))
            {
                Console.WriteLine(file);
                lines.Add($"\n\n // {file}");
                string text = System.IO.File.ReadAllText(file);
                text += "// end of file \n";
                importSection = true;
                string header="";
                do
                {
                    int eol = text.IndexOf("\r");
                    
                    if(eol != -1){
                        header = text.Substring(0,eol);
                        text = text.Substring(eol);
                        if(text.Length>0 && text[0]!='}')
                            text=text.Substring(1);
                    }
                    else
                    {
                        header = "";
                        break;
                    }
                   // Console.WriteLine(header);
                     
                    if( header.IndexOf("class")!=-1 )
                        importSection = false;
                    else
                        declarations+=header;
                    if(importSection == false)
                        lines.Add(header);
                    header = "";

                    
                }while(text.IndexOf("\n")!=-1);  
                lines.Add(text);
             

            }
            

            src = $"namespace {ns}"+"{\n    /***/";
            foreach(string file in lines)
            {
                if(file.IndexOf("{")!=-1)tab+="  ";
                if(file.IndexOf("}")!=-1)tab=tab.Substring(0,tab.Length-2);
                //Console.WriteLine(tab.Length + src);
                src += file.Trim().Replace("\n","").Replace("\r","")+"\n"+tab;


                //Console.ReadLine();
            }
            src += "\n} // end of {ns} \n";
            Console.WriteLine(src);
            System.IO.File.WriteAllText(output, src);
        }

        static void Tst1(string[] args)
        {
            /*CopyCatalogs(
                @"C:\admin\k.batov.io@outlook.com\Projects_2\RunJS",
                @"D:\Projects-Console\CsNamespaceHelper",
                @"D:\Projects-Console\RunJS");*/
            //PutToNamespae("Common",@"D:\Common", @"D:\CommonNameSpace.cs");
        }

        private static void CopyCatalogs(string dir1,string dir2,string dir3)
        {
            foreach (var file in GetCsSrcFiles(
                            dir1))
            {
                //System.Console.WriteLine(file);
                string relative = file.Substring(dir1.Length + 1);
                string[] ids = relative.Split(@"\");

                string dir = dir1;

                if (System.IO.Directory.Exists(dir) == false)
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                for (int i = 0; i < ids.Length - 1; i++)
                {
                    dir += @"\" + ids[i];
                    string targetDir = dir.Replace(dir1,dir3);
                    if (System.IO.Directory.Exists(targetDir) == false)
                    {
                        System.IO.Directory.CreateDirectory(targetDir);
                    }
                    Console.WriteLine(targetDir);
                }
                string destPath = file.Replace( dir2, dir3);
                string text = System.IO.File.ReadAllText(file);
                System.IO.File.WriteAllText(destPath, text);
            }
        }

        static void Run(string[] args)
        {
            
            try
            {
                Console.WriteLine(new DOS().Execute(@"d: && fc /n d:\1.json d:\2.json"));

                Thread.Sleep(2000);
                /*new HostedAppUnitTest().DoTest();
                System.Console.BackgroundColor = ConsoleColor.Black;
                System.Console.ForegroundColor = ConsoleColor.White;
                TestingProgram.Run(args);*/
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetType() + "-" + ex.TargetSite + "=" + ex.Source + "-" + ex.Message);
                ex.StackTrace.WriteToConsole();
            }
        }

    }
}
