namespace GitBuildInfo.SourceGenerator.Tests;

public class VBGeneratorTest : VisualBasicSourceGeneratorTest<SourceGenerator, XUnitVerifier>, IGeneratorTestBase
{
    public List<(string, string)> GlobalOptions { get; } = new();

    protected override GeneratorDriver CreateGeneratorDriver(Project project, ImmutableArray<ISourceGenerator> sourceGenerators)
        => VisualBasicGeneratorDriver.Create(
            sourceGenerators,
            project.AnalyzerOptions.AdditionalFiles,
            (VisualBasicParseOptions)project.ParseOptions!,
            new OptionsProvider(project.AnalyzerOptions.AnalyzerConfigOptionsProvider, this.GlobalOptions));
}
