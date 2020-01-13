namespace NP.Concepts.ComponentFolders
{
    public interface IComponentMetaDataContainer
    {
        bool IsFolder { get; }

        bool IsMatching { get; }

        void CheckMatching(string strToMatch);
    }

    public interface IComponentMetaDataContainer<TMetaData> : 
        IComponentMetaDataContainer
        where TMetaData : IComponentDisplayMetadata
    {

        TMetaData MetaData { get; }
    }
}
