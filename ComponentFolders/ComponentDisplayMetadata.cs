namespace NP.Concepts.ComponentFolders
{
    public abstract class ComponentDisplayMetadata : ComponentDisplayMetadataBase
    {
        public abstract bool IsFolder { get; }

        public ComponentDisplayMetadata
        (
            string displayName,
            string icon,
            string description)
            :
            base(displayName, icon, description)
        {

        }

        public ComponentDisplayMetadata
        (
            ComponentDisplayMetadataBase componentDisplayMetadata
        ) 
            :
            base(componentDisplayMetadata)
        {

        }
    }
}
