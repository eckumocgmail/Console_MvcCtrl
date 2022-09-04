using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FileUnit: TestingUnit
{
    public FileUnit()
    {
        Push(new FileResourceTest());
 

        Push(new DirectoryTest());
    }
}
