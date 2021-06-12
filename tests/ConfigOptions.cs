namespace GitBuildInfo.SourceGenerator.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Allows adding additional global options
    /// </summary>
    internal class ConfigOptions : AnalyzerConfigOptions
    {
        private readonly AnalyzerConfigOptions _workspaceOptions;
        private readonly Dictionary<string, string> _globalOptions;

        public ConfigOptions(AnalyzerConfigOptions workspaceOptions, List<(string, string)> globalOptions)
        {
            _workspaceOptions = workspaceOptions;
            _globalOptions = globalOptions.ToDictionary(t => t.Item1, t => t.Item2);
        }

        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
            => _workspaceOptions.TryGetValue(key, out value) || _globalOptions.TryGetValue(key, out value);
    }
}
