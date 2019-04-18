using System;
using System.Linq;
using System.Reflection;

namespace AssemblyDependencyChecker
{
    class Program
    {
        private static Configuration _configuration = new Configuration
        {
            AssemblyToCheck = @"C:/full/path/to/your/assembly.dll",
            IgnoreDuplicates = true,
            IgnoreVersionZero = true
        };

        private static AssemblyWalker _assemblyWalker = new AssemblyWalker(_configuration);

        static void Main(string[] args)
        {
            ConsoleUtils.WriteLine($"Loading: {_configuration.AssemblyToCheck}");
            var assembly = Assembly.LoadFile(_configuration.AssemblyToCheck);

            _assemblyWalker.LoadReferencedAssemblies(assembly);

            //PrintTree(); // Order alphabetically
            CheckForMultipleVersionsOfSameAssembly();

            // TODO: Failed to load the following assemblies (therefore, their dependencies couldn't be evaluated):
            // (order alphabetically)

            ConsoleUtils.WriteLine("\nDone.");
            Console.Read();
        }

        private static void CheckForMultipleVersionsOfSameAssembly()
        {
            var assembliesWithMultipleVersions = _assemblyWalker.Assemblies
                .OrderBy(kvp => kvp.Key)
                .Where(kvp => kvp.Value.Count > 1)
                .ToList();

            foreach (var keyValuePair in assembliesWithMultipleVersions)
            {
                ConsoleUtils.WriteLine($"\nMultiple versions of assembly {keyValuePair.Key} are referenced.");

                foreach (var reference in keyValuePair.Value)
                {
                    ConsoleUtils.WriteLine(1, $"Version {reference.Version} referenced by:", ConsoleColor.Yellow);

                    foreach (var dep in reference.ReferencedBy)
                    {
                        ConsoleUtils.WriteLine(2, $"{dep.FullName}", ConsoleColor.DarkYellow);
                    }
                }
            }
        }
    }
}
