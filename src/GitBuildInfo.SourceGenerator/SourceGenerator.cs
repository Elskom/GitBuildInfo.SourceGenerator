namespace GitBuildInfo.SourceGenerator;

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
        if (context.Compilation is not CSharpCompilation compilation)
        {
            return;
        }

        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.GitBuildInfoAssemblyType", out var assemblyType);
        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.GitBuildInfoIsGeneric", out var isGeneric);
        var gitHead = context.AdditionalFiles.First(text => text.Path.EndsWith("git_head.txt")).GetText()?.ToString();
        var commitHash = context.AdditionalFiles.First(text => text.Path.EndsWith("git_commit_hash.txt")).GetText()?.ToString();
        var gitBranch = context.AdditionalFiles.First(text => text.Path.EndsWith("git_branch.txt")).GetText()?.ToString();
        context.AddSource(
            "GitAssemblyInfo.g.cs",
            SourceText.From(
                Generator.CreateAndGenerateCode(
                    new GeneratorOptions
                    {
                        RootNamespace = rootNamespace,
                        AssemblyType = assemblyType,
                        IsGeneric = Convert.ToBoolean(isGeneric),
                        IsCSharp10OrGreater = compilation.LanguageVersion is LanguageVersion.CSharp10
                            or LanguageVersion.Latest
                            or LanguageVersion.Preview,
                    },
                    new GitInfo
                    {
                        GitHead = gitHead!.Trim(Environment.NewLine.ToCharArray()),
                        CommitHash = commitHash!.Trim(Environment.NewLine.ToCharArray()),
                        GitBranch = gitBranch!.Trim(Environment.NewLine.ToCharArray()),
                    },
                    context),
                Encoding.UTF8));
    }
}
