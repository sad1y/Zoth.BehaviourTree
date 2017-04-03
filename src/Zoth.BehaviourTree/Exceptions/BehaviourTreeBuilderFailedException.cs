using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeBuilderFailedException : BehaviourTreeException
    {
        public BehaviourTreeBuilderFailedException(string message) : base(message)
        {
        }
    }
}
