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

        // copy
        public XamlResourceInfo(XamlResourceInfo xamlResourceInfo) : 
            this(xamlResourceInfo.ResourceDictionaryUrl, xamlResourceInfo.ResourceKey)
        {
        }

        public bool IsValid =>
            (!ResourceDictionaryUrl.IsNullOrEmpty()) && 
            (!ResourceKey.IsNullOrEmptyCollection());
    }
}
