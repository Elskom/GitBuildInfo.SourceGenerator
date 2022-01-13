namespace GitBuildInfo.SourceGenerator;

internal class GeneratorOptions
{
    internal static readonly DiagnosticDescriptor ValidationWarning = new(
        "GITINFO000",
        "GitBuildInfoSourceGeneratorConfigurationValidationWarning",
        "{0} should not be an empty string",
        "Functionality",
        DiagnosticSeverity.Warning,
        true);

    public string RootNamespace { get; set; }

    public string AssemblyType { get; set; }

    public bool IsGeneric { get; set; }

    internal bool IsCSharp10OrGreater { get; set; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Validate(GeneratorExecutionContext context)
    {
        if (string.IsNullOrEmpty(this.AssemblyType))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    ValidationWarning,
                    null,
                    nameof(this.AssemblyType)));
            throw new InvalidOperationException(
                string.Format(
                    ValidationWarning.MessageFormat.ToString(),
                    nameof(this.AssemblyType)));
        }
    }
}
