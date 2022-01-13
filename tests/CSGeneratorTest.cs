namespace GitBuildInfo.SourceGenerator.Tests;

public class CSGeneratorTest : CSharpSourceGeneratorTest<SourceGenerator, XUnitVerifier>, IGeneratorTestBase
{
    public List<(string, string)> GlobalOptions { get; } = new();

    public Microsoft.CodeAnalysis.CSharp.LanguageVersion LanguageVersion { get; set; }

    protected override GeneratorDriver CreateGeneratorDriver(Project project, ImmutableArray<ISourceGenerator> sourceGenerators)
        => CSharpGeneratorDriver.Create(
            sourceGenerators,
            project.AnalyzerOptions.AdditionalFiles,
            (CSharpParseOptions)this.CreateParseOptions(),
            new OptionsProvider(project.AnalyzerOptions.AnalyzerConfigOptionsProvider, this.GlobalOptions));

    protected override ParseOptions CreateParseOptions()
        => ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(this.LanguageVersion);
}
