namespace GitBuildInfo.SourceGenerator.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// This class just passes argument through to the projects options provider and it used to provider custom global options
    /// </summary>
    internal class OptionsProvider : AnalyzerConfigOptionsProvider
    {
        private readonly AnalyzerConfigOptionsProvider _analyzerConfigOptionsProvider;

        public OptionsProvider(AnalyzerConfigOptionsProvider analyzerConfigOptionsProvider, List<(string, string)> globalOptions)
        {
            _analyzerConfigOptionsProvider = analyzerConfigOptionsProvider;
            GlobalOptions = new ConfigOptions(_analyzerConfigOptionsProvider.GlobalOptions, globalOptions);
        }

        public override AnalyzerConfigOptions GlobalOptions { get; }

        [ExcludeFromCodeCoverage]
        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
            => _analyzerConfigOptionsProvider.GetOptions(tree);

        [ExcludeFromCodeCoverage]
        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
            => _analyzerConfigOptionsProvider.GetOptions(textFile);
    }
}
