using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeCompilationFailedException : Exception
    {
        public BehaviourTreeCompilationFailedException(string message) : base(message)
        {
        }
    }
}
