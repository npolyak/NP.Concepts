namespace NP.Concepts
{
    public interface IConverter<TIn, TOut>
    {
        TOut Convert(TIn val);
    }

    public interface IConverter : IConverter<object, object> { }
}
