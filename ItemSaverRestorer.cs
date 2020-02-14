using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using NP.Utilities.FolderUtils;

namespace NP.Concepts
{
    public class ItemSaverRestorer
    {
        public ItemToFolderSaverRestorer SaverRestorer { get; }

        public string BaseDirName { get; }

        public string DefaultItemFileName { get; }

        public string ItemsDirName { get; }

        public string FileExtension { get; }

        private (string fileName, string dirName) GetItemPath(string itemName)
        {
            if (itemName.IsNullOrWhiteSpace())
            {
                return (DefaultItemFileName, null);
            }

            if (FileExtension != null)
            {
                itemName = itemName + "." + FileExtension;
            }

            return (itemName, ItemsDirName);
        }

        public ItemSaverRestorer
        (
            string baseDirName,
            string defaultItemFileName, 
            string fileExtension = null,
            string itemsDirName = "CustomAssemblies")
        {
            BaseDirName = baseDirName;
            DefaultItemFileName = defaultItemFileName;
            FileExtension = fileExtension;
            ItemsDirName = itemsDirName;

            SaverRestorer = new ItemToFolderSaverRestorer(BaseDirName);
        }

        public void Restore
        (
            IStrRestorable objToRestore,
            string itemName)
        {
            (var fileName, var dirName) = GetItemPath(itemName);

            string strToRestoreObjFrom =
                SaverRestorer.RestoreStr(fileName, dirName);

            if (objToRestore is IItemNameContainer itemNameContainer)
            {
                itemNameContainer.ItemName = itemName;
            }

            objToRestore.Restore(strToRestoreObjFrom);
        }

        public void Save(IStrSaveableRestorableClearable objToSave, string itemName)
        {
            (var fileName, var dirName) = GetItemPath(itemName);

            string strToSave = 
                objToSave.Save();

            SaverRestorer.SaveStr(fileName, strToSave, dirName);

            if (objToSave is ISaveable saveable)
            {
                saveable.OnSave(itemName);
            }
        }

        public void Delete(IStrSaveableRestorableClearable objToDelete, string itemName)
        {
            (var fileName, var dirName) = GetItemPath(itemName);

            if (objToDelete is IDeletable deletable)
            {
                deletable.OnDelete(itemName);
            }

            SaverRestorer.DeleteItem(fileName, dirName);
        }
    }
}
