namespace GitBuildInfo.SourceGenerator.Tests;

/// <summary>
/// This class just passes argument through to the projects options provider and it used to provider custom global options
/// </summary>
internal class OptionsProvider : AnalyzerConfigOptionsProvider
{
    private readonly AnalyzerConfigOptionsProvider _analyzerConfigOptionsProvider;

    public OptionsProvider(AnalyzerConfigOptionsProvider analyzerConfigOptionsProvider, List<(string, string)> globalOptions)
    {
        _analyzerConfigOptionsProvider = analyzerConfigOptionsProvider;
        this.GlobalOptions = new ConfigOptions(_analyzerConfigOptionsProvider.GlobalOptions, globalOptions);
    }

    public override AnalyzerConfigOptions GlobalOptions { get; }

    [ExcludeFromCodeCoverage]
    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        => _analyzerConfigOptionsProvider.GetOptions(tree);

    [ExcludeFromCodeCoverage]
    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        => _analyzerConfigOptionsProvider.GetOptions(textFile);
}
