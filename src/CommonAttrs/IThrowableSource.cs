using System;
using System.Collections;

namespace ValidationAnnotationsNS
{
    public interface IThrowableSource
    {
        IDictionary GetData();
        IDisposable[] GetStack();
        void ThrowData(string message);
    }
}