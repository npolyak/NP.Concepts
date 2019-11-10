using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using NP.Utilities.FolderUtils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentLoader<TId, TAssemblyAttribute, TClassAttribute>
        where TId : INameContainer
        where TAssemblyAttribute : Attribute, IComponentDisplayMetadata
        where TClassAttribute : Attribute, IComponentDisplayMetadata
    {
        public ComponentFolder<TId> RootFolder { get; }

        public Func<string, TId> IdFactory { get; }

        public ComponentLoader
        (
            string rootFolderName, 
            string locationUnderProgramData,
            Func<string, TId> idFactory)
        {
            IdFactory = idFactory;

            RootFolder = new ComponentFolder<TId>(rootFolderName, null, null);

            AppFolderUtils folderUtils = new AppFolderUtils(locationUnderProgramData);
            string fullBasePath = folderUtils.FullBasePath;

            var dirs = Directory.EnumerateDirectories(fullBasePath);
            foreach (string dirName in dirs)
            {
                string localDirName = dirName.SubstrFromTo("\\", null, false);

                string filePath = $"{dirName}\\{localDirName}.dll";

                if (File.Exists(filePath))
                {
                    Assembly assembly = Assembly.ReflectionOnlyLoadFrom(filePath);

                    var datumAssemblyCustomAttribute =
                        assembly.GetCustomAttributesData()
                                .FirstOrDefault(attrData => attrData.AttributeType.FullName == typeof(TAssemblyAttribute).FullName);

                    if (datumAssemblyCustomAttribute != null)
                    {
                        assembly = Assembly.LoadFile(filePath);

                        TAssemblyAttribute datumAssemblyAttribute =
                            assembly.GetCustomAttributes<TAssemblyAttribute>().FirstOrDefault();

                        string shortName = datumAssemblyAttribute.DisplayName ??
                            assembly.Location.SubstrFromTo("\\", ".", false);

                        ComponentFolder<TId> subFolder =
                            RootFolder.GetOrAddFolder
                            (
                                shortName,
                                datumAssemblyAttribute.Icon,
                                datumAssemblyAttribute.Description);

                        foreach (Type typeFromAssembly in assembly.DefinedTypes)
                        {
                            if (!typeFromAssembly.IsPublic || typeFromAssembly.IsGenericType || typeFromAssembly.IsAbstract)
                                continue;

                            TClassAttribute datumProcessorAttribute =
                                typeFromAssembly.GetCustomAttribute<TClassAttribute>();

                            if (datumProcessorAttribute != null)
                            {
                                string fullTypeName = typeFromAssembly.FullName;

                                TId bbId = IdFactory(fullTypeName);

                                shortName =
                                    datumProcessorAttribute.DisplayName ?? fullTypeName.SubstrFromTo(".", null, false);

                                bbId.AddDisplayMetaData
                                (
                                    datumProcessorAttribute.Icon,
                                    shortName,
                                    datumProcessorAttribute.Description);

                                subFolder.Add(bbId);
                            }
                        }
                    }
                }
            }
        }
    }
}
