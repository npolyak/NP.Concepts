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
        //public ComponentFolder<TId, TFolderMetaData, TComponentMetaData> RootFolder { get; }

            //public Func<Type, TId> IdFactory { get; }

            //private Func<Assembly, TAssemblyAttribute, TMetaData> _folderMetadataFactory;
            //private string _locationUnderProgramData;
            //Func<Assembly, IEnumerable<(Type, TMetaData)>> _typeAndMetadataGetter;
            //Func<Type, TMetaData, TMetaData> _componentMetaDataFactory;

            //public ComponentLoader
            //(
            //    string locationUnderProgramData,
            //    Func<Type, TId> idFactory,
            //    TMetaData rootFolderMetaData,
            //    Func<Assembly, TAssemblyAttribute, TMetaData> folderMetadataFactory,
            //    Func<Assembly, IEnumerable<(Type, TMetaData)>> typeAndMetadataGetter,
            //    Func<Type, TMetaData, TMetaData> componentMetaDataFactory)
            //{
            //    _locationUnderProgramData = locationUnderProgramData;

            //    IdFactory = idFactory;

            //    RootFolder =
            //        new ComponentFolder<TId, TMetaData>(rootFolderMetaData);

            //    _folderMetadataFactory = folderMetadataFactory;

            //    _typeAndMetadataGetter = typeAndMetadataGetter;
            //    _componentMetaDataFactory = componentMetaDataFactory;
            //}

            //public void Load<TComponentIdWithDisplayMetadata>()
            //    where TComponentIdWithDisplayMetadata : IComponentIdWithDisplayMetadata<TId, TMetaData>
            //{

            //    ComponentFolder<TId, TMetaData> currentFolder = this.RootFolder;

            //    foreach (Assembly assembly in _locationUnderProgramData.LoadComponentDlls<TAssemblyAttribute>())
            //    {
            //        TAssemblyAttribute loadedAssemblyAttribute =
            //            assembly.GetCustomAttributes<TAssemblyAttribute>().FirstOrDefault();

            //        currentFolder =
            //            currentFolder.GetOrAddFolder(_folderMetadataFactory(assembly, loadedAssemblyAttribute));

            //        foreach ((Type typeFromAssembly, TMetaData typeProcessorAttribute) in _typeAndMetadataGetter(assembly))
            //        {
            //            TId bbId = IdFactory(typeFromAssembly);

            //            TMetaData metaData =  _componentMetaDataFactory(typeFromAssembly, typeProcessorAttribute);

            //            TComponentIdWithDisplayMetadata componentInfo =
            //                (TComponentIdWithDisplayMetadata) Activator.CreateInstance(typeof(TComponentIdWithDisplayMetadata), bbId, metaData);

            //            currentFolder.Add(componentInfo);
            //        }
            //    }
            //}
    }
}
