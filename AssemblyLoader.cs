using NP.Utilities;
using NP.Utilities.FolderUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NP.Concepts
{
    public static class AssemblyLoader
    {
        static Dictionary<string, object> _locationsToLoadAssembliesFrom = new Dictionary<string, object>();

        private static Dictionary<Type, object> _assemblyAttrTypes = new Dictionary<Type, object>();
        private static MetadataLoadContext TheMetadataLoadContext;


        public static void AddAssemblyAttrType(this Type assemblyAttrType)
        {
            if (!typeof(Attribute).IsAssignableFrom(assemblyAttrType))
            {
                throw new Exception($"Programming Error: Type '{assemblyAttrType.FullName}' is not an Attribute");
            }

            _assemblyAttrTypes[assemblyAttrType] = assemblyAttrType;
        }

        private static bool _initialized = false;

        public static void Init()
        {
            if (!_initialized)
            {
                _initialized = true;

                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            }
        }


        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly requestingAssembly = args.RequestingAssembly;

            string assemblyName = args.GetAssemblyNameFromAssemblyResolveArgs();

            if (requestingAssembly == null)
            {
                AppDomain appDomain = sender as AppDomain;

                if (appDomain == null)
                {
                    return null;
                }

                string path = appDomain.BaseDirectory + assemblyName + ".dll";

                if (File.Exists(path))
                {
                    return Assembly.LoadFrom(path);
                }

                return null;
            }

            string assemblyLocation = requestingAssembly.Location.SubstrFromTo(null, "\\", false);

            if ( _locationsToLoadAssembliesFrom.ContainsKey(assemblyLocation) ||
                 requestingAssembly.CustomAttributes.Any(attr => _assemblyAttrTypes.ContainsKey(attr.AttributeType)))
            {
                _locationsToLoadAssembliesFrom[assemblyLocation] = assemblyLocation;

                string path = $"{assemblyLocation}\\{assemblyName}.dll";

                if (File.Exists(path))
                {
                    return Assembly.LoadFile(path);
                }
            }

            return null;
        }

        private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);

            return Assembly.ReflectionOnlyLoad(assemblyName.FullName);
        }

        static AssemblyLoader()
        {
            Init();
        }

        private static IEnumerable<string> GetPluginFilePaths(this IEnumerable<string> dirs)
        {
            return dirs.Select(dirName =>
            {
                string localDirName = dirName.SubstrFromTo("\\", null, false);

                string filePath = $"{dirName}\\{localDirName}.dll";

                return filePath;
            })
            .Where(filePath => File.Exists(filePath));
        }

        public static IEnumerable<Assembly>
            LoadComponentDlls(this string locationUnderProgramData, Type assemblyAttributeType)
        {
            AppFolderUtils folderUtils =
                new AppFolderUtils(locationUnderProgramData);

            string fullBasePath = folderUtils.FullBasePath;

            var dirs = Directory.EnumerateDirectories(fullBasePath);

            List<string> pluginFilePaths = dirs.GetPluginFilePaths().ToList();

            //TheMetadataLoadContext = new MetadataLoadContext(new PathAssemblyResolver(pluginFilePaths));

            foreach(var filePath in pluginFilePaths)
            {
                //Assembly assembly = 
                //    TheMetadataLoadContext.LoadFromAssemblyPath(filePath);

                //CustomAttributeData loadAssemblyAttributeData =
                //        assembly.GetCustomAttributesData()
                //                .FirstOrDefault(attrData => attrData.AttributeType.FullName == typeof(TLoadAssemblyAttribute).FullName);

               // if (loadAssemblyAttributeData != null)
                {
                    Assembly assembly = Assembly.LoadFile(filePath);

                    if (assembly.GetCustomAttributesData()
                                .Any(attrData => attrData.AttributeType.FullName == assemblyAttributeType.FullName))
                    {
                        yield return assembly;
                    }
                }
            }
        }

        public static IEnumerable<Assembly>
            LoadComponentDlls<TAssemblyAttribute>(this string locationUnderProgramData)
            where TAssemblyAttribute : Attribute
        {
            return locationUnderProgramData.LoadComponentDlls(typeof(TAssemblyAttribute));
        }


        public static IEnumerable<(Type type, TClassAttribute classAttr)> GetAttributedTypes<TClassAttribute>(this Assembly assembly)
            where TClassAttribute : Attribute
        {
            foreach (Type typeFromAssembly in assembly.DefinedTypes)
            {
                if (!typeFromAssembly.IsPublic ||
                     typeFromAssembly.IsGenericType ||
                     typeFromAssembly.IsAbstract)
                {
                    continue;
                }

                TClassAttribute typeProcessorAttribute =
                    typeFromAssembly.GetCustomAttribute<TClassAttribute>();

                if (typeProcessorAttribute != null)
                {
                    yield return (typeFromAssembly, typeProcessorAttribute);
                }
            }
        }
    }
}
