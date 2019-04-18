namespace AssemblyDependencyChecker
{
    public class Configuration
    {
        /// <summary>
        /// The path to the DLL to analyze.
        /// </summary>
        public string AssemblyToCheck { get; set; }

        /// <summary>
        /// Whether to ignore when the same version of an assembly is referenced multiple times.
        /// If true, only shows each version of an assembly once.
        /// </summary>
        public bool IgnoreDuplicates { get; set; }

        /// <summary>
        /// Whether to ignore version 0.0.0.0 when searching for duplicates.
        /// If true, will not consider version 0.0.0.0 of assembly to be distinct.
        /// </summary>
        public bool IgnoreVersionZero { get; set; }
    }
}
