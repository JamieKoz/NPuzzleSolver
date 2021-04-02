using System;

namespace NPuzzle.Exceptions
{
    public class CantMoveThatWayException : Exception
    {
        public CantMoveThatWayException()
        {
        }

        public CantMoveThatWayException(string message)
            : base(message)
        {
        }

        public CantMoveThatWayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}