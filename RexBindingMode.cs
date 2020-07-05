namespace NP.Concepts
{
    public enum RexBindingMode
    {
        None,
        OneWay,
        TwoWay
    }

    public static class RexBindingModeHelper
    {
        public static RexBindingMode[] RexBindingModes { get; } =
            { RexBindingMode.None, RexBindingMode.OneWay, RexBindingMode.TwoWay };
    }
}
