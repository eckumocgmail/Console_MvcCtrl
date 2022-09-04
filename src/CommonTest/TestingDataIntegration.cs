using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

public abstract class TestingDataIntegration : TestingElement,IDisposable
{
    protected readonly object _context;

    public TestingDataIntegration(object context)
    {
        _context = context;
    }

 

    protected abstract void onDataContextTest(object db);
    protected override void OnTest()
    {
        onDataContextTest(_context);
    }

    public new void Dispose()
    {
        if(_context is IDisposable)
        {
            ((IDisposable)_context).Dispose();
        }
        
    }
}