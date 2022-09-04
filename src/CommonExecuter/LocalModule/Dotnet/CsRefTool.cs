using System;
using System.Collections.Generic;

namespace CsNamespaceHelper
{
    class CsRefTool
    {


        /// <summary>
        /// 
        /// </summary>        
        public static void Run(string[] args)
        {
            PutToNamespace("DateValAttrsNS",
                @"D:\gitlab\auth\Data\Resources\Common\ValidationAnnotations\Date",
                @"D:\gitlab\auth\Data\Resources\Common\dateval-attrs.module.cs");
            PutToNamespace("NumValAttrsNS",
                @"D:\gitlab\auth\Data\Resources\Common\ValidationAnnotations\Number",
                @"D:\gitlab\auth\Data\Resources\Common\numval-attrs.module.cs");
            PutToNamespace("TextValAttrsNS",
                @"D:\gitlab\auth\Data\Resources\Common\ValidationAnnotations\Text",
                @"D:\gitlab\auth\Data\Resources\Common\textval-attrs.module.cs");
          
        }




        //пути ко всем исходникам в директории
        private static List<string> GetCsSrcFiles(string dir)
        {
            List<string> files = new List<string>();
            foreach (string path in System.IO.Directory.GetFiles(dir))
            {
                if (path.EndsWith(".cs"))
                {
                    files.Add(path);
                }
            }
            foreach (string subdir in System.IO.Directory.GetDirectories(dir))
            {
                files.AddRange(GetCsSrcFiles(subdir));
            }
            return files;
        }



        //пути ко всем исходникам в директории
        public static void PutToNamespace(string ns, string dir, string output)
        {
            int tab = 0;
            string src = "";
            bool importSection = true;
            var lines = new List<string>();
            string declarations = "";
            foreach (string file in GetCsSrcFiles(dir))
            {
                System.Console.WriteLine(file);

                string text = System.IO.File.ReadAllText(file);
                 
                importSection = true;
                string header = "";
                do
                {
                    int eol = text.IndexOf("\n");

                    if (eol != -1)
                    {
                        header = text.Substring(0, eol);
                        
                        text = text.Substring(eol);
                        if (text.Length > 0 && text[0]!='}')
                            text = text.Substring(1);
                    }
                    else
                    {
                        if(importSection == false)
                            lines.Add(text);
                        header = "";
                        break;
                    }
                    // System.Console.WriteLine(header);

                    if (header.IndexOf("class") != -1 || header.IndexOf("enum") != -1 || header.IndexOf("interface") != -1)
                        importSection = false;
                    else
                        declarations += header;
                    if (importSection == false && header.IndexOf("using")==-1)
                        lines.Add(header);
                    header = "";


                } while (text.IndexOf("\n") != -1);

                lines.Add(" \n \n \n ");
            }


            src = $"namespace {ns}" + "{\n";
            tab += 1;
            foreach (string file in lines)
            {
                if (file.IndexOf("{") != -1) tab += 1;
                if (file.IndexOf("}") != -1) tab -= 1;
                //System.Console.WriteLine("                         ".Substring(0,tab*2) + src);
                src += file.Replace("\n", "") + "\n";                
            }
            src += "\n} // end of {ns} \n";
            tab -= 1;
            System.Console.WriteLine(src);
            System.IO.File.WriteAllText(output, src);
        }


    }
}
