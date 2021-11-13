using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class ForbiddenAccessException : Exception, ISerializable
    {
        public ForbiddenAccessException()
        {
        }

        public ForbiddenAccessException(string message) : base(message)
        {
        }

        public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ForbiddenAccessException(string name, object key) : base($"Role \"{name}\" ({key}) không tồn tại ")
        {
        }

        protected ForbiddenAccessException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}