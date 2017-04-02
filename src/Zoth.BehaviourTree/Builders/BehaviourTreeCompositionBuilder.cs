using System;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Builders
{
    internal class BehaviourTreeCompositeNodeBuilder<TTickData, TState> : IBehaviourTreeBuilder<TTickData, TState>
    {
        private readonly Stack<IBehaviourTreeCompositeNode<TTickData, TState>> _parentNodeStack = new Stack<IBehaviourTreeCompositeNode<TTickData, TState>>();
        private IBehaviourTreeCompositeNode<TTickData, TState> _currentNode;

        public BehaviourTreeCompositeNodeBuilder(IBehaviourTreeCompositeNode<TTickData, TState> parentNode)
        {
            if(parentNode == null)
                throw new ArgumentNullException(nameof(parentNode));

            _parentNodeStack.Push(parentNode);
        }

        public IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeNode<TTickData, TState> node)
        {
            if(node == null)
                throw new ArgumentNullException(nameof(node));

            _parentNodeStack.Peek().AddNode(node);
            return this;
        }

        public IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeCompositeNode<TTickData, TState> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _parentNodeStack.Push(node);
            return this;
        }

        public IBehaviourTreeBuilder<TTickData, TState> End()
        {
            if (_parentNodeStack.Count == 1)
                return this;

            _currentNode = _parentNodeStack.Pop();
            return this;
        }
    }
}
