using System;

namespace NPuzzle.Exceptions
{
    public class InvalidPuzzleException : Exception
    {
        public InvalidPuzzleException()
        {
        }

        public InvalidPuzzleException(string message)
            : base(message)
        {
        }

        public InvalidPuzzleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}