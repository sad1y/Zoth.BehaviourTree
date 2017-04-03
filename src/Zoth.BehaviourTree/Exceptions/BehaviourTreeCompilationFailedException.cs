using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeCompilationFailedException : BehaviourTreeException
    {
        public BehaviourTreeCompilationFailedException(string message) : base(message)
        {
        }
    }
}
