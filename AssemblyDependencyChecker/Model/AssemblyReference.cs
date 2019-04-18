using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyDependencyChecker.Model
{
    /// <summary>
    /// Information on an assembly reference to another assembly.
    /// </summary>
    public class AssemblyReference
    {
        /// <summary>
        /// The referenced assembly.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// The version referenced.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The list of assemblies referencing this assembly.
        /// </summary>
        public List<AssemblyName> ReferencedBy { get; set; }
    }
}
