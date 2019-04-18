using AssemblyDependencyChecker.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyDependencyChecker
{
    /// <summary>
    /// Class that recurses through assemblies to find their dependencies.
    /// </summary>
    public class AssemblyWalker
    {
        public Dictionary<string, List<AssemblyReference>> Assemblies = new Dictionary<string, List<AssemblyReference>>();

        private Configuration _configuration;

        public AssemblyWalker(Configuration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Recursively load dependencies of an Assembly.
        /// </summary>
        public void LoadReferencedAssemblies(Assembly assembly, int depth = 0)
        {
            foreach (AssemblyName referencedAssemblyInfo in assembly.GetReferencedAssemblies())
            {
                AddToLoadedAssemblies(assembly, referencedAssemblyInfo);

                if (_configuration.IgnoreDuplicates && AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == referencedAssemblyInfo.FullName))
                {
                    // Skip this assembly because it's already been loaded
                    continue;
                }

                ConsoleUtils.WriteLine(depth, $" {referencedAssemblyInfo}");

                try
                {
                    var nextAssembly = Assembly.Load(referencedAssemblyInfo);
                    LoadReferencedAssemblies(nextAssembly, depth + 1);
                }
                catch (FileNotFoundException e)
                {
                    ConsoleUtils.WriteLine(depth, $" Error: Unable to load: {referencedAssemblyInfo}", ConsoleColor.Red);
                }
            }
        }

        private void AddToLoadedAssemblies(Assembly parentAssembly, AssemblyName assemblyInfo)
        {
            var assemblyName = assemblyInfo.Name;
            var assemblyVersion = assemblyInfo.Version;

            // Get the list of versions we've already seen
            var referencesToThisAssembly = Assemblies.ContainsKey(assemblyName)
                ? Assemblies[assemblyName]
                : new List<AssemblyReference>();

            var referencesToThisAssemblyVersion = referencesToThisAssembly.SingleOrDefault(r => r.Version == assemblyVersion);
            if (referencesToThisAssemblyVersion != null)
            {
                // We have already seen this version, add the parent assembly to the list of referencing assemblies
                if (!referencesToThisAssemblyVersion.ReferencedBy.Contains(assemblyInfo))
                {
                    referencesToThisAssemblyVersion.ReferencedBy.Add(parentAssembly.GetName());
                }
            }
            else
            {
                // This is a new version
                referencesToThisAssemblyVersion = new AssemblyReference
                {
                    Assembly = assemblyName,
                    Version = assemblyVersion,
                    ReferencedBy = new List<AssemblyName>
                    {
                        parentAssembly.GetName()
                    }
                };

                referencesToThisAssembly.Add(referencesToThisAssemblyVersion);
                Assemblies[assemblyName] = referencesToThisAssembly;
            }
        }
    }
}
