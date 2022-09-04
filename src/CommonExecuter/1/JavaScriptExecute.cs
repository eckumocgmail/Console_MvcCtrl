 
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace RunJS
{


    class JavaScriptExecute
    {
        

        private static string JAVA_SCRIPT_APPLICATION_FILE = "index.js";




        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Execute(string code)
        {

            return Execute(new string[1] { code });
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Execute(string[] args)
        {
            WriteToFile(args);
            string output = CommandExecute("node " + JAVA_SCRIPT_APPLICATION_FILE);

            return output.TrimEnd();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private static void WriteToFile(string[] args)
        {
            string code = "";
            foreach (string arg in args)
            {
                code += arg + " \n";
            }
            System.IO.File.WriteAllText(JAVA_SCRIPT_APPLICATION_FILE, code);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static string CommandExecute(string command)
        {
            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            string response = process.StandardOutput.ReadToEnd();
            return response;
        }



 
    }
}
