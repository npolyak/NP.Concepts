namespace NP.Concepts.Behaviors
{
    public interface ISearchable
    {
        string SearchStr { get; set; }

        string SearchableStr { get; }
    }

    public static class SearchableUtils
    {
        public static bool SearchStrFound(this ISearchable searchable)
        {
            return string.IsNullOrWhiteSpace(searchable?.SearchStr) ||
                    searchable.SearchableStr?.Contains(searchable.SearchStr) == true;
        }
    }
}
