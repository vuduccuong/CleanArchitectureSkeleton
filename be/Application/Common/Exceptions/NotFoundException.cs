using System;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innderException) :
            base(message, innderException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found!")
        {
        }
    }
}
