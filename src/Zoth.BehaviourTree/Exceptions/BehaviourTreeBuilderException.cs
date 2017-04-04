using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeBuilderException : BehaviourTreeException
    {
        public BehaviourTreeBuilderException(string message) : base(message)
        {
        }
    }
}
