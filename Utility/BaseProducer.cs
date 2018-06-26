using System;
using CompilerContract;

namespace RoslynCompiler
{
    public class BaseProducer : IProducer
    {
        public virtual object Run()
        {
            throw new NotImplementedException();
        }
    }
}
