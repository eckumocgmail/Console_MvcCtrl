using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/// <summary>
/// Анализатор процесса зарегистрированного в операционной системе
/// </summary>
public class ProcessAnalitic: EcKuMoC
{
    public static void TraceComponents( Process proc )
    {
        foreach (var module in proc.Modules)
        {
            System.Console.WriteLine(((ProcessModule)module).BaseAddress);
            System.Console.WriteLine(((ProcessModule)module).FileName);
            if (((ProcessModule)module).FileName.EndsWith(".exe") == false)
            {
                try
                {
                    foreach (var ptype in System.Reflection.Assembly.LoadFile(((ProcessModule)module).FileName).GetTypes())
                    {
                        System.Console.WriteLine(" " + ptype.Name);

                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            

        }
        if (proc.Container != null)
        {
            WriteLine("Компоненты процесса: ");
            foreach (var component in proc.Container.Components)
            {
                WriteLine("  " + component.GetType().Name);
            }
        }
    }
}