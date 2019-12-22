using NP.Utilities;

namespace NP.Concepts
{
    public class XamlResourceInfo
    {
        public string ResourceDictionaryUrl { get; }
        public object ResourceKey { get; }

        public XamlResourceInfo(string resourceDictionaryUrl, object resourceKey)
        {
            ResourceDictionaryUrl = resourceDictionaryUrl;
            ResourceKey = resourceKey;
        }

        public bool IsValid =>
            (!ResourceDictionaryUrl.IsNullOrEmpty()) && 
            (!ResourceKey.IsNullOrEmptyCollection());
    }
}
