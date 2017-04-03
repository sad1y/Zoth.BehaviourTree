using System;

namespace Zoth.BehaviourTree.Exceptions
{
    public class BehaviourTreeException : Exception 
    {
        public BehaviourTreeException(string message) : base(message)
        {

        }
    }
}
