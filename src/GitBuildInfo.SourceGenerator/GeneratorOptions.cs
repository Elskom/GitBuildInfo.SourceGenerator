namespace GitBuildInfo.SourceGenerator
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.CodeAnalysis;

    public record GeneratorOptions
    {
        internal static readonly DiagnosticDescriptor ValidationWarning = new(
            "GITINFO000",
            "GitBuildInfoSourceGeneratorConfigurationValidationWarning",
            "{0} should not be an empty string",
            "Functionality",
            DiagnosticSeverity.Warning,
            true);

        public string RootNamespace { get; init; }

        public string AssemblyType { get; init; }

        public bool IsGeneric { get; init; }

        internal bool IsCSharp10OrGreater { get; init; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Validate(GeneratorExecutionContext context)
        {
            if (string.IsNullOrEmpty(this.AssemblyType))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        ValidationWarning,
                        null,
                        nameof(AssemblyType)));
                throw new InvalidOperationException(
                    string.Format(
                        ValidationWarning.MessageFormat.ToString(),
                        nameof(AssemblyType)));
            }
        }
    }
}
