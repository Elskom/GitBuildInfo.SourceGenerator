namespace GitBuildInfo.SourceGenerator
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.Json.Serialization;

    public record GeneratorOptions
    {
        [JsonPropertyName(nameof(AssemblyType))]
        public string? AssemblyType { get; init; }

        [JsonPropertyName(nameof(IsGeneric))]
        public bool IsGeneric { get; init; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Validate()
        {
            if (string.IsNullOrEmpty(this.AssemblyType))
            {
                throw new InvalidOperationException("AssemblyType should not be null or an empty string.");
            }
        }
    }
}
