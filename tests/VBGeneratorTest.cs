namespace GitBuildInfo.SourceGenerator.Tests
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing.Verifiers;
    using Microsoft.CodeAnalysis.VisualBasic;
    using Microsoft.CodeAnalysis.VisualBasic.Testing;

    public class VBGeneratorTest : VisualBasicSourceGeneratorTest<SourceGenerator, XUnitVerifier>, IGeneratorTestBase
    {
        public List<(string, string)> GlobalOptions { get; } = new();

        protected override GeneratorDriver CreateGeneratorDriver(Project project, ImmutableArray<ISourceGenerator> sourceGenerators)
            => VisualBasicGeneratorDriver.Create(
                sourceGenerators,
                project.AnalyzerOptions.AdditionalFiles,
                (VisualBasicParseOptions)project.ParseOptions!,
                new OptionsProvider(project.AnalyzerOptions.AnalyzerConfigOptionsProvider, GlobalOptions));
    }
}
