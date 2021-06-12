namespace GitBuildInfo.SourceGenerator
{
    public record GitInfo
    {
        public string GitHead { get; init; }

        public string CommitHash { get; init; }

        public string GitBranch { get; init; }
    }
}
