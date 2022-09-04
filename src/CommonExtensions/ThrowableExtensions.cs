using COM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ThrowableExtensions
{

    public static void Fix(this Exception ex)
    {
        WinSysAPI.InfoDialog("Источник",ex.Source);
        WinSysAPI.EditTextFile(ex.Source);

    }
}