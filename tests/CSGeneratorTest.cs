namespace GitBuildInfo.SourceGenerator.Tests
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Testing;
    using Microsoft.CodeAnalysis.Testing.Verifiers;

    public class CSGeneratorTest : CSharpSourceGeneratorTest<SourceGenerator, XUnitVerifier>, IGeneratorTestBase
    {
        public List<(string, string)> GlobalOptions { get; } = new();

        protected override GeneratorDriver CreateGeneratorDriver(Project project, ImmutableArray<ISourceGenerator> sourceGenerators)
            => CSharpGeneratorDriver.Create(
                sourceGenerators,
                project.AnalyzerOptions.AdditionalFiles,
                (CSharpParseOptions)project.ParseOptions!,
                new OptionsProvider(project.AnalyzerOptions.AnalyzerConfigOptionsProvider, GlobalOptions));
    }
}
