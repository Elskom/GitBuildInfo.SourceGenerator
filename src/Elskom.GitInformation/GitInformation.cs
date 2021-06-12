// Copyright (c) 2019-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    /// Obtain the git repository information for the assembly.
    /// </summary>
    public class GitInformation
    {
        // This is the collection of instances this has.
        private static readonly Dictionary<Assembly, GitInformation> AssemblyInstances = new();
        private static readonly HashSet<Assembly> AppliedAssemblies = new();

        private GitInformation(string headdesc, string commit, string branchname)
        {
            this.Headdesc = headdesc;
            this.Commit = commit;
            this.Branchname = branchname;
        }

        /// <summary>
        /// Gets the git commit head description based upon the string constructed by
        /// git name-rev.
        /// </summary>
        public string Headdesc { get; }

        /// <summary>
        /// Gets the git commit hash as formatted by git rev-parse.
        /// </summary>
        /// <value>
        /// The git commit hash as formatted by git rev-parse.
        /// </value>
        public string Commit { get; }

        /// <summary>
        /// Gets the git branch name as formatted by git name-rev.
        /// </summary>
        /// <value>
        /// The git branch name as formatted by git name-rev.
        /// </value>
        public string Branchname { get; }

        /// <summary>
        /// Gets a value indicating whether the branch is dirty or
        /// clean based upon the string constructed by git describe.
        /// </summary>
        /// <value>
        /// A value indicating whether the branch is dirty or
        /// clean based upon the string constructed by git describe.
        /// </value>
        public bool IsDirty => this.Headdesc.EndsWith("-dirty", StringComparison.Ordinal);

        /// <summary>
        /// Gets a value indicating whether the branch is the master
        /// branch or not based upon the string constructed by
        /// git name-rev. This also returns true if the branch is main as well.
        /// </summary>
        /// <value>
        /// A value indicating whether the branch is the master
        /// branch or not based upon the string constructed by
        /// git name-rev. This also returns true if the branch is main as well.
        /// </value>
        [Obsolete("Use GitInformation.IsMain instead. This will be removed in a future release. This is because most people using git are abandoning the use of master as the default branch name for the name of main. To prevent breakage I suggest you rename your default branch from master to main today.")]
        [ExcludeFromCodeCoverage]
        public bool IsMaster => this.Branchname.Equals("master", StringComparison.Ordinal) || this.IsMain;

        /// <summary>
        /// Gets a value indicating whether the branch is the main
        /// branch or not based upon the string constructed by
        /// git name-rev.
        /// </summary>
        /// <value>
        /// A value indicating whether the branch is the main
        /// branch or not based upon the string constructed by
        /// git name-rev.
        /// </value>
        public bool IsMain => this.Branchname.Equals("main", StringComparison.Ordinal);

        /// <summary>
        /// Gets a value indicating whether refs point to a stable tag release.
        /// </summary>
        /// <value>
        /// A value indicating whether refs point to a stable tag release.
        /// </value>
        public bool IsTag => this.Headdesc.StartsWith("refs/tags", StringComparison.Ordinal);

        /// <summary>
        /// Applies the <see cref="Attribute"/>s that the specified <see cref="Assembly"/> contains.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly"/> to apply the attributes to.
        /// </param>
        public static void ApplyAssemblyAttributes(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            // this check is to avoid a stack overflow exception.
            if (AppliedAssemblies.Add(assembly))
            {
                _ = assembly.GetCustomAttributes(false);
            }
        }

        /// <summary>
        /// Gets the instance of the <see cref="GitInformation"/> class for
        /// the specified <see cref="Type"/> or <see langword="null"/>.
        /// </summary>
        /// <param name="assemblyType">
        /// The <see cref="Type"/> to the <see cref="Assembly"/> to look for a instance of the <see cref="GitInformation"/> class.
        /// </param>
        /// <returns>
        /// The <see cref="GitInformation"/> class instance to the specified <see cref="Type"/>
        /// or <see langword="null"/>.
        /// </returns>
        public static GitInformation GetAssemblyInstance(Type assemblyType)
            => assemblyType is null
                ? throw new ArgumentNullException(nameof(assemblyType))
                : GetAssemblyInstance(assemblyType.Assembly);

        /// <summary>
        /// Gets the instance of the <see cref="GitInformation"/> class for
        /// the specified <see cref="Assembly"/> or <see langword="null"/>.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly"/> to look for a instance of the <see cref="GitInformation"/> class.
        /// </param>
        /// <returns>
        /// The <see cref="GitInformation"/> class instance to the specified <see cref="Assembly"/>
        /// or <see langword="null"/>.
        /// </returns>
        public static GitInformation GetAssemblyInstance(Assembly assembly)
            => AssemblyInstances.TryGetValue(assembly, out var gitInformation) ? gitInformation : null;

        internal static void GetOrCreateAssemblyInstance(string headdesc, string commit, string branchname, Assembly assembly)
        {
            if (GetAssemblyInstance(assembly) is null && !AssemblyInstances.ContainsKey(assembly))
            {
                AssemblyInstances.Add(assembly, new GitInformation(headdesc, commit, branchname));
            }
        }
    }
}
