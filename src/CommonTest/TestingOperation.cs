using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
public class TestingOperation: TestingElement 
{
    private Action ToTest = null;

  

    public TestingOperation(string Name, Action toTest)
    {
        //ID = Name;
        ToTest = toTest;
    }

    protected override void OnTest()
    {
        this.ToTest();
    }
}
 
