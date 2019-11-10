using NP.Utilities;
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
    }
}
