namespace GitBuildInfo.SourceGenerator
{
    using System.Text.Json.Serialization;

    public record GeneratorOptions
    {
        [JsonPropertyName("AssemblyType")]
        public string? AssemblyType { get; set; }

        [JsonPropertyName("IsGeneric")]
        public bool IsGeneric { get; set; }
    }
}
