namespace GitBuildInfo.SourceGenerator
{
    using System.Text.Json.Serialization;

    public record GitInfo
    {
        [JsonPropertyName(nameof(GitHead))]
        public string? GitHead { get; init; }

        [JsonPropertyName(nameof(CommitHash))]
        public string? CommitHash { get; init; }

        [JsonPropertyName(nameof(GitBranch))]
        public string? GitBranch { get; init; }
    }
}
