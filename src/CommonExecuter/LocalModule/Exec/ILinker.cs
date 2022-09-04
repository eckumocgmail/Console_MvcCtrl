using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exec
{
    class ILinker
    {
        public string CreateLink()
        {
            return Exec(@"

function make-link ($target, $link) {
    New-Item -Path $link -ItemType SymbolicLink -Value $target
} ");
        }

        private string Exec(string v)
        {
            throw new NotImplementedException();
        }
    }
}
