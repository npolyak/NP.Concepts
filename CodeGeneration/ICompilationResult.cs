using System;
using System.Collections.Generic;
using System.Text;

namespace NP.Concepts.CodeGeneration
{
    public interface ICompilationResult
    {
        byte[] TheResult { get; }

        bool Success { get; }

        string TheOutputName { get; }
    }
}
