namespace GitBuildInfo.SourceGenerator.Tests
{
    using System;
    using Elskom.Generic.Libs;
    using Xunit;

    public class GitInformationTests
    {
        public GitInformationTests()
        {
            GitInformation.ApplyAssemblyAttributes(typeof(GitInformation).Assembly);
        }

        [Fact]
        public void TestApplyNullAssembly()
        {
            Assert.Throws<ArgumentNullException>(() => GitInformation.ApplyAssemblyAttributes(null));
        }

        [Fact]
        public void TestGetInstanceNotNull()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            _ = typeof(GitInformation).Assembly.GetCustomAttributes(false);
            Assert.NotNull(instance);
        }

        [Fact]
        public void TestGetInstanceNull()
        {
            // always null because the assembly that provides System.String does not have an instance.
            var instance = GitInformation.GetAssemblyInstance(typeof(string));
            Assert.Null(instance);
        }

        [Fact]
        public void TestGetInstanceThrows()
        {
            Assert.Throws<ArgumentNullException>(() => GitInformation.GetAssemblyInstance((Type?)null));
        }

        [Fact]
        public void TestInstanceBranchnameNotEmpty()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            Assert.NotEmpty(instance!.Branchname);
        }

        [Fact]
        public void TestInstanceCommitNotEmpty()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            Assert.NotEmpty(instance!.Commit);
        }

        [Fact]
        public void TestInstanceHeaddescNotEmpty()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            Assert.NotEmpty(instance!.Headdesc);
        }

        [Fact]
        public void TestInstanceIsDirty()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            Assert.Equal(instance!.IsDirty, instance.IsDirty);
        }

        [Fact]
        public void TestInstanceIsMain()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            Assert.Equal(instance!.IsMain, instance.IsMain);
        }

        [Fact]
        public void TestInstanceIsTag()
        {
            var instance = GitInformation.GetAssemblyInstance(typeof(GitInformation));
            Assert.Equal(instance!.IsTag, instance.IsTag);
        }
    }
}
