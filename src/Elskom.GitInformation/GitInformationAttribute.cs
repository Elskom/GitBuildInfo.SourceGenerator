// Copyright (c) 2019-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;

    // This attribute can only be used on an assembly.

    /// <summary>
    /// Attribute that creates and registers a instance of the <see cref="GitInformation" /> class for the assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public class GitInformationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitInformationAttribute"/> class.
        /// </summary>
        /// <param name="headdesc">
        /// The head description of the git repository to the assembly.
        /// </param>
        /// <param name="commit">
        /// The commit to the git repository to the assembly.
        /// </param>
        /// <param name="branchname">
        /// The branch name to the git repository to the assembly.
        /// </param>
        /// <param name="assemblyType">
        /// The <see langword="type"/> of the <see langword="assembly"/> <see langword="this"/> attribute <see langword="is"/> applied to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="assemblyType"/> is <see langword="null"/>.
        /// </exception>
        public GitInformationAttribute(string headdesc, string commit, string branchname, Type assemblyType)
        {
            GitInformation.ApplyAssemblyAttributes(typeof(GitInformation).Assembly);
            GitInformation.GetOrCreateAssemblyInstance(headdesc, commit, branchname, assemblyType.Assembly);
        }
    }
}
