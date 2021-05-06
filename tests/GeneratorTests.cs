﻿namespace GitBuildInfo.SourceGenerator.Tests
{
    using GitBuildInfo.SourceGenerator;
    using Xunit;

    public class GeneratorTests
    {
        [Fact]
        public void TestGeneratingNonGeneric()
        {
            var result = DoTest("TestNamespace.Test", false);
            Assert.Equal($@"// <autogenerated/>
using Elskom.Generic.Libs;
using TestNamespace;

[assembly: GitInformationAttribute(""fbgtgretgtre"", ""vfdbttregter"", ""vsdfvfdsv"", typeof(Test))]
", result);
        }

        [Fact]
        public void TestGeneratingGeneric()
        {
            var result = DoTest("TestNamespace.Test", true);
            Assert.Equal($@"// <autogenerated/>
using Elskom.Generic.Libs;
using TestNamespace;

[assembly: GitInformationAttribute(""fbgtgretgtre"", ""vfdbttregter"", ""vsdfvfdsv"", typeof(Test<>))]
", result);
        }

        [Fact]
        public void TestGeneratingFailure()
            => Assert.Throws<GenerationFailedException>(() => DoTest(string.Empty, false));

        // the values on GitInfo are not valid in terms of results
        // that would normally be from git.
        private static string DoTest(string assemblyType, bool generic)
            => Generator.CreateAndGenerateCode(
                $@"{{
  ""$schema"": ""https://raw.githubusercontent.com/Elskom/GitBuildInfo.SourceGenerator/main/settings.schema.json"",
  ""AssemblyType"": ""{assemblyType}"",
  ""IsGeneric"": {generic.ToString().ToLowerInvariant()},
}}",
                @"{
  ""GitHead"": ""fbgtgretgtre"",
  ""CommitHash"": ""vfdbttregter"",
  ""GitBranch"": ""vsdfvfdsv"",
}");
    }
}
