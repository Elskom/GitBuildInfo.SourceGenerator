namespace GitBuildInfo.SourceGenerator
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class GenerationFailedException : Exception
    {
        public GenerationFailedException()
        {
        }

        public GenerationFailedException(string message)
            : base(message)
        {
        }

        public GenerationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GenerationFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
