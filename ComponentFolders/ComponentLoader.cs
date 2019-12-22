using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentLoader<TId, TMetaData, TAssemblyAttribute>
        where TId : INameContainer
        where TMetaData : class, IComponentDisplayMetadata
        where TAssemblyAttribute : Attribute, TMetaData
    {
        public ComponentFolder<TId, TMetaData> RootFolder { get; }

        public Func<Type, TId> IdFactory { get; }

        private Func<Assembly, TAssemblyAttribute, TMetaData> _folderMetadataFactory;
        private string _locationUnderProgramData;
        Func<Assembly, IEnumerable<(Type, TMetaData)>> _typeAndMetadataGetter;
        Func<Type, TMetaData, TMetaData> _componentMetaDataFactory;

        public ComponentLoader
        (
            string locationUnderProgramData,
            Func<Type, TId> idFactory,
            TMetaData rootFolderMetaData,
            Func<Assembly, TAssemblyAttribute, TMetaData> folderMetadataFactory,
            Func<Assembly, IEnumerable<(Type, TMetaData)>> typeAndMetadataGetter,
            Func<Type, TMetaData, TMetaData> componentMetaDataFactory)
        {
            _locationUnderProgramData = locationUnderProgramData;

            IdFactory = idFactory;

            RootFolder =
                new ComponentFolder<TId, TMetaData>(rootFolderMetaData);

            _folderMetadataFactory = folderMetadataFactory;

            _typeAndMetadataGetter = typeAndMetadataGetter;
            _componentMetaDataFactory = componentMetaDataFactory;
        }

        public void Load<TComponentIdWithDisplayMetadata>()
            where TComponentIdWithDisplayMetadata : IComponentIdWithDisplayMetadata<TId, TMetaData>
        {
            foreach (Assembly assembly in _locationUnderProgramData.LoadComponentDlls<TAssemblyAttribute>())
            {
                TAssemblyAttribute loadedAssemblyAttribute =
                    assembly.GetCustomAttributes<TAssemblyAttribute>().FirstOrDefault();

                ComponentFolder<TId, TMetaData> subFolder =
                    RootFolder.GetOrAddFolder(_folderMetadataFactory(assembly, loadedAssemblyAttribute));

                foreach ((Type typeFromAssembly, TMetaData typeProcessorAttribute) in _typeAndMetadataGetter(assembly))
                {
                    TId bbId = IdFactory(typeFromAssembly);

                    TMetaData metaData =  _componentMetaDataFactory(typeFromAssembly, typeProcessorAttribute);

                    TComponentIdWithDisplayMetadata componentInfo =
                        (TComponentIdWithDisplayMetadata) Activator.CreateInstance(typeof(TComponentIdWithDisplayMetadata), bbId, metaData);

                    subFolder.Add(componentInfo);
                }
            }
        }
    }
}
