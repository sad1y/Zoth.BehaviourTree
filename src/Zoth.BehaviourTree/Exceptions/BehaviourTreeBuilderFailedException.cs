using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeBuilderFailedException : Exception
    {
        public BehaviourTreeBuilderFailedException(string message) : base(message)
        {
        }
    }
}
