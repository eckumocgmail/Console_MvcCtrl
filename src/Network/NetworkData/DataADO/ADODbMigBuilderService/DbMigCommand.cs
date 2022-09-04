using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataADO
{
    public class DbMigCommand
    {
        public int ID { get; set; }
        public int Version { get; set; }
        public string Up { get; set; }
        public string Down { get; set; }

        public DbMigCommand(string up, string down, int version)
        {
            Up = up;
            Down = down;
            Version = version;
        }
    }
}
