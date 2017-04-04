using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeCompilationException : BehaviourTreeException
    {
        public BehaviourTreeCompilationException(string message) : base(message)
        {
        }
    }
}
