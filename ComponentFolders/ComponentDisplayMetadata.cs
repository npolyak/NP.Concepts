namespace NP.Concepts.ComponentFolders
{
    public interface IComponentDisplayMetadata
    {
        string DisplayName { get; }

        string Icon { get; }

        string Description { get; }
    }

    public class ComponentDisplayMetadata : IComponentDisplayMetadata
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

        public void CopyFrom(IComponentDisplayMetadata componentDisplayMetadata)
        {
            SetFrom
            (
                componentDisplayMetadata.DisplayName, 
                componentDisplayMetadata.Icon, 
                componentDisplayMetadata.Description);
        }

        public ComponentDisplayMetadata
        (
            string displayName,
            string icon,
            string description)
        {
            SetFrom(displayName, icon, description);
        }

        public ComponentDisplayMetadata(IComponentDisplayMetadata componentDisplayMetadata) :
            this
            (
                componentDisplayMetadata.DisplayName,
                componentDisplayMetadata.Icon,
                componentDisplayMetadata.Description)
        {

        }
    }
}
