using NP.Utilities;
using System.Reflection;

namespace NP.Concepts.CodeGeneration
{
    public static class DynamicLoader
    {
        public const string GENERATED_NAMESPACE_NAME = "Generated";

        public static object GetDynamicObjByTypeName(string dynamicType)
        {
            string fullDynamicTypeName =
                StrUtils.StrConcat
                (
                    new[] { GENERATED_NAMESPACE_NAME, dynamicType }, 
                    null,
                    StrUtils.PLAIN_PROP_PATH_LINK_SEPARATOR
                );

            object result = TheGeneratedAssembly.CreateInstance(fullDynamicTypeName);

            return result;
        }

        public static Assembly TheGeneratedAssembly { get; private set; }

        public static void CreateAssembly(this ICompilationResult result)
        {
            if (result.Success)
            {
                TheGeneratedAssembly = Assembly.Load(result.TheResult);
            }
        }

        public static void AddModuleCompilation(this ICompilationResult moduleResult)
        {
            if (!moduleResult.Success)
                return;

            TheGeneratedAssembly.LoadModule
            (
                moduleResult.TheOutputName,
                moduleResult.TheResult
            );
        }

        public static void AddModuleCompilations(params ICompilationResult[] moduleResults)
        {
            if (moduleResults == null)
                return;

            moduleResults
                .DoForEach(moduleResult => AddModuleCompilation(moduleResult));
        }
    }
}
