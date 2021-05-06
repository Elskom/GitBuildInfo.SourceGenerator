namespace GitBuildInfo.SourceGenerator.Tests
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json;
    using GitBuildInfo.SourceGenerator;
    using Xunit;

    public class GenerationFailedExceptionTests
    {
        [Fact]
        public void TestDefaultThrow()
            => Assert.Throws<GenerationFailedException>(ThrowDefault);

        [Fact]
        public void TestWithInnerException()
            => Assert.Throws<GenerationFailedException>(ThrowWithInnerException);

        [Fact]
        public void TestSerializer()
        {
            var ex = new GenerationFailedException();
            try
            {
                var json = JsonSerializer.Serialize(ex);
                var deserializedException = JsonSerializer.Deserialize<GenerationFailedException>(json);
                throw deserializedException!;
            }
            catch (SerializationException)
            {
                throw new InvalidOperationException("Unable to serialize/deserialize the exception");
            }
            catch (GenerationFailedException)
            {
                // expected
            }
        }

        private static void ThrowDefault()
            => throw new GenerationFailedException();

        private static void ThrowWithInnerException()
        {
            try
            {
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException ex)
            {
                throw new GenerationFailedException("Testing", ex);
            }
        }
    }
}
