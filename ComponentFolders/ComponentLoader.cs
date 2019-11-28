using NP.Utilities.BasicInterfaces;
using System;
using System.Linq;
using System.Reflection;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentLoader<TId, TMetaData, TAssemblyAttribute, TClassAttribute>
        where TId : INameContainer
        where TMetaData : class, IComponentDisplayMetadata
        where TAssemblyAttribute : Attribute, TMetaData
        where TClassAttribute : Attribute, TMetaData
    {
        public ComponentFolder<TId, TMetaData> RootFolder { get; }

        public Func<Type, TId> IdFactory { get; }

        public ComponentLoader
        (
            string locationUnderProgramData,
            Func<Type, TId> idFactory,
            TMetaData rootFolderMetaData,
            Func<Assembly, TAssemblyAttribute, TMetaData> folderMetadataFactory,
            Func<Type, TClassAttribute, TMetaData> componentMetaDataFactory)
        {
            IdFactory = idFactory;

            RootFolder =
                new ComponentFolder<TId, TMetaData>(rootFolderMetaData);

            foreach(Assembly assembly in locationUnderProgramData.LoadComponentDlls<TAssemblyAttribute>())
            {
                TAssemblyAttribute loadedAssemblyAttribute =
                    assembly.GetCustomAttributes<TAssemblyAttribute>().FirstOrDefault();

                ComponentFolder<TId, TMetaData> subFolder =
                    RootFolder.GetOrAddFolder(folderMetadataFactory(assembly, loadedAssemblyAttribute));

                foreach ((Type typeFromAssembly, TClassAttribute typeProcessorAttribute) in assembly.GetAttributedTypes<TClassAttribute>())
                {
                    TId bbId = IdFactory(typeFromAssembly);

                    bbId.AddDisplayMetaData<TId, TMetaData>
                    (
                        componentMetaDataFactory(typeFromAssembly, typeProcessorAttribute));

                    subFolder.Add(bbId);
                }
            }
        }
    }
}
