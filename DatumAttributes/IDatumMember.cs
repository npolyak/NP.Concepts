using System;

namespace NP.Concepts.DatumAttributes
{
    public interface IDatumMember
    {
        DatumPropertyDirection Direction { get; }

        Type DatumType { get; }

        bool IsSaveable { get; }

        bool PropagateWhenConnected { get; }

        bool IsNotifiable { get; }

        bool IsType { get; }
    }
}
