namespace GitBuildInfo.SourceGenerator
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Source Generator for dumping git build information into a assembly level attribute on the compilation.
    /// </summary>
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            // Source Generators do not need to fill this in.
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation is not CSharpCompilation)
            {
                return;
            }

            var gitBuildInfoJsonFile = context.AdditionalFiles
                .FirstOrDefault(af => string.Equals(Path.GetFileName(af.Path), "GitBuildInfo.json", StringComparison.OrdinalIgnoreCase));
            var gitInfoJsonFile = context.AdditionalFiles
                .FirstOrDefault(af => string.Equals(Path.GetFileName(af.Path), "GitInfo.json", StringComparison.OrdinalIgnoreCase));
            if (gitBuildInfoJsonFile is null || gitInfoJsonFile is null)
            {
                return;
            }

            context.AddSource(
                "GitAssemblyInfo.g.cs",
                SourceText.From(
                    Generator.CreateAndGenerateCode(
                        gitBuildInfoJsonFile.GetText(context.CancellationToken)!.ToString(),
                        gitInfoJsonFile.GetText(context.CancellationToken)!.ToString()),
                    Encoding.UTF8));
        }
    }
}
