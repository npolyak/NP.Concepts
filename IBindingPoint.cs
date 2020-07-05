namespace NP.Concepts
{
    public interface IBindingPoint
    {
        RexBindingMode AllowedBinding { get; }

        public string UniqueName { get; }
    }
}
