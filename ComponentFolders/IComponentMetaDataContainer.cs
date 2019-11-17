namespace NP.Concepts.ComponentFolders
{
    public interface IComponentMetaDataContainer<TMetaData>
        where TMetaData : IComponentDisplayMetadata
    {
        bool IsFolder { get; }

        TMetaData MetaData { get; }

        bool IsMatching { get; }

        void CheckMatching(string strToMatch);
    }
}
