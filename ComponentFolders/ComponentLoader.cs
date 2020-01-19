using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NP.Concepts.ComponentFolders
{
    public static class ComponentLoader
    {
        public static IEnumerable<(ComponentFolder<TId, TFolderMetaData, TComponentMetaData>, Assembly)> LoadAssemblies<TId, TFolderMetaData, TComponentMetaData>
        (
            this ComponentFolder<TId, TFolderMetaData, TComponentMetaData> currentFolder, 
            string locationUnderProgramData,
            Func<Assembly, ComponentFolder<TId, TFolderMetaData, TComponentMetaData>, ComponentFolder<TId, TFolderMetaData, TComponentMetaData>> assemblyFolderFactory,
            Type assemblyAttributeType
        )
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            foreach (Assembly assembly in locationUnderProgramData.LoadComponentDlls(assemblyAttributeType))
            {
                yield return (assemblyFolderFactory(assembly, currentFolder), assembly);
            }
        }

        public static void LoadComponents<TId, TFolderMetaData, TComponentMetaData>
        (
            this ComponentFolder<TId, TFolderMetaData, TComponentMetaData> currentFolder,
            Assembly assembly,
            Func<Assembly, IEnumerable<IComponentIdWithDisplayMetadata<TId, TComponentMetaData>>> componentFactory
        )
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            foreach (IComponentIdWithDisplayMetadata<TId, TComponentMetaData> componentInfo in componentFactory(assembly))
            {
                currentFolder.Add(componentInfo);
            }
        }

        public static void Load<TId, TFolderMetaData, TComponentMetaData>
        (
            this ComponentFolder<TId, TFolderMetaData, TComponentMetaData> currentFolder,
            string locationUnderProgramData,
            Type assemblyAttributeType,
            Func<Assembly, ComponentFolder<TId, TFolderMetaData, TComponentMetaData>, ComponentFolder<TId, TFolderMetaData, TComponentMetaData>> assemblyFolderFactory,
            Func<Assembly, IEnumerable<IComponentIdWithDisplayMetadata<TId, TComponentMetaData>>> componentFactory
        )
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            var foldersAndAssemblies = 
                currentFolder.LoadAssemblies(locationUnderProgramData, assemblyFolderFactory, assemblyAttributeType);

            foreach(var (folder, assembly) in foldersAndAssemblies)
            {
                folder.LoadComponents(assembly, componentFactory);
            }
        }
    }
}
