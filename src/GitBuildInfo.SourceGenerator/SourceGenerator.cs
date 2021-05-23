namespace GitBuildInfo.SourceGenerator
{
    using System;
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

            _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.GitBuildInfoAssemblyType", out var assemblyType);
            _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.GitBuildInfoIsGeneric", out var isGeneric);
            _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.GitHead", out var gitHead);
            _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.CommitHash", out var commitHash);
            _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.GitBranch", out var gitBranch);
            context.AddSource(
                "GitAssemblyInfo.g.cs",
                SourceText.From(
                    Generator.CreateAndGenerateCode(
                        new GeneratorOptions { AssemblyType = assemblyType, IsGeneric = Convert.ToBoolean(isGeneric) },
                        new GitInfo { GitHead = gitHead, CommitHash = commitHash, GitBranch = gitBranch },
                        context),
                    Encoding.UTF8));
        }
    }
}
