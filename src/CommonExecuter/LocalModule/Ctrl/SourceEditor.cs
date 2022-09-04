using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverapp
{
    interface SourceEditor
    {
        public string Path { get; set; }



        /// <summary>
        /// Вырезать реализация класса из файла
        /// </summary>
        /// <returns></returns>
        public string GetClassSource();


    }








    class A
    {
        //пути ко всем исходникам в директории
        private static List<string> GetCsSrcFiles(string dir)
        {
            List<string> files = new List<string>();
            foreach (string path in System.IO.Directory.GetFiles(dir))
            {
                if (path.EndsWith(".cs") && path.EndsWith(".cshtml") == false)
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




        static void LastMain(string[] args)
        {


            ForEachCsSrc((kv) =>
            {
                bool isHeader = true;
                string header = "";
                foreach (string line in kv.Value.Split("\n"))
                {
                    if (isHeader)
                    {
                        if (line.Contains("namespace") ||
                            line.Contains(@"//") ||
                            line.Contains("/") ||
                            line.Contains("class") ||
                            line.Contains("interface") ||
                            line.Contains("enum"))
                        {
                            header += line + "\n";
                            isHeader = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(line.Trim()) == false)
                            {
                                string l = line;
                                while (l.IndexOf("using") != -1) l = l.Replace("using", "");
                                header += ("using " + l + ";\n").Replace(";;", ";");
                            }
                        }
                    }
                    else
                    {
                        header += line + "\n";
                    }

                }


                //Console.Clear();
                //Console.WriteLine(header);
                //if (string.IsNullOrEmpty(Console.ReadLine()))
                //{
                System.IO.File.WriteAllText(kv.Key, header);
                //}

            });
        }

        public static void RePairUsing()
        {
            ForEachCsSrc((kv) =>
            {
                string str = "";
                GetUsings(kv.Key).ToList().ForEach(line => str += line + ";\n");

                kv.Value.Split("\n").Where(line => line.Contains("using ") == false).ToList().ForEach(line => str += line + "\n");
                System.IO.File.WriteAllText(kv.Key, str);
            });
        }
        public static void ForEachLineInCsSrc(Action<KeyValuePair<string, List<string>>> todo, string projectDir = @"D:\gitlab\auth")
        {
            foreach (var file in GetCsSrcFiles(projectDir))
            {
                string text = System.IO.File.ReadAllText(file);
                todo(new KeyValuePair<string, List<string>>(file, new List<string>(text.Split("\n"))));
            }
        }
        public static void ForEachCsSrc(Action<KeyValuePair<string, string>> todo, string projectDir = @"D:\gitlab\auth")
        {
            foreach (var file in GetCsSrcFiles(projectDir))
            {
                string text = System.IO.File.ReadAllText(file);
                todo(new KeyValuePair<string, string>(file, text));
            }
        }


        private static void AppendCodeToUsingSection(string usings, string projectDir = @"D:\gitlab\auth")
        {
            foreach (var file in GetCsSrcFiles(projectDir))
            {
                string text = System.IO.File.ReadAllText(file);
                System.IO.File.WriteAllText(file, usings + text);
            }
        }

        public static HashSet<string> GetUsings(string file)
        {
            string text = System.IO.File.ReadAllText(file);
            return new HashSet<string>(text.Split("\n").ToList().Where(line => line.Trim().StartsWith("using ")).ToList().Select(line => line.Substring("using ".Length).Trim().Replace(";", "")));
        }
    }
}
