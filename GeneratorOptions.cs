namespace GitBuildInfo.SourceGenerator
{
    /// <summary>
    /// Describes the options that feed into code generation.
    /// </summary>
    public record GeneratorOptions
    {
        /// <summary>
        /// Gets the type to use to apply the attribute to embed the git information in that is within the assembly it is being applied to.
        /// </summary>
        /// <value>The type to use to apply the attribute to embed the git information in that is within the assembly it is being applied to.</value>
        public string? AssemblyType { get; set; }

        /// <summary>
        /// Gets if the type specified in AssemblyType is a generic type, by default this is set to false to indicate that the type is not a generic type.
        /// </summary>
        /// <value>If the type specified in AssemblyType is a generic type, by default this is set to false to indicate that the type is not a generic type.</value>
        public bool IsGeneric { get; set; }
    }
}
