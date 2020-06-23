using System;

namespace NP.Concepts.DatumAttributes
{
    public interface IDatumMember
    {
        DatumPropertyDirection Direction { get; }

        bool IsSaveable { get; }

        bool PropagateWhenConnected { get; }

        bool IsType { get; }

        bool IsCollection { get; }

        bool IsPullAlways { get; }
    }
}
