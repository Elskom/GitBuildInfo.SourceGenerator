namespace GitBuildInfo.SourceGenerator
{
    using System.Text.Json.Serialization;

    public record GitInfo
    {
        [JsonPropertyName("GitHead")]
        public string? GitHead { get; set; }

        [JsonPropertyName("CommitHash")]
        public string? CommitHash { get; set; }

        [JsonPropertyName("GitBranch")]
        public string? GitBranch { get; set; }
    }
}
