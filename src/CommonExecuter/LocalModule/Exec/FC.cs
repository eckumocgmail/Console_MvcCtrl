using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tools;

namespace Exec
{
    public class FC
    {
        private PowerShell ps { get; set; } = new PowerShell("D:\\");

        public FC()
        {

        }


        public string Compare(string f1, string f2)
        {
            return ps.Execute($"fc /n {f1} {f2}");
        }

    }
}
