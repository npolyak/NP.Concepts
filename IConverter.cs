namespace NP.Concepts
{
    public interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn val);
    }

    public interface IConverter : IConverter<object, object> { }
}
