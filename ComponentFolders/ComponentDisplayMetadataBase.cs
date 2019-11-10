using NP.Utilities.BasicInterfaces;

namespace NP.Concepts.ComponentFolders
{
    public interface IComponentDisplayMetadata
    {
        string DisplayName { get; }

        string Icon { get; }

        string Description { get; }
    }

    public class ComponentDisplayMetadataBase : ICopyable<ComponentDisplayMetadataBase>, IComponentDisplayMetadata
    {
        public string DisplayName { get; private set; }

        public string Icon { get; private set; }

        public string Description { get; private set; }

        private void SetFrom
        (
            string displayName,
            string icon,
            string description)
        {
            DisplayName = displayName;
            Icon = icon;
            Description = description;
        }

        public void CopyFrom(ComponentDisplayMetadataBase componentDisplayMetadata)
        {
            SetFrom(componentDisplayMetadata.DisplayName, componentDisplayMetadata.Icon, componentDisplayMetadata.Description);
        }

        public ComponentDisplayMetadataBase
        (
            string displayName,
            string icon,
            string description)
        {
            SetFrom(displayName, icon, description);
        }

        public ComponentDisplayMetadataBase(ComponentDisplayMetadataBase componentDisplayMetadata) :
            this
            (
                componentDisplayMetadata.DisplayName,
                componentDisplayMetadata.Icon,
                componentDisplayMetadata.Description)
        {

        }
    }
}
