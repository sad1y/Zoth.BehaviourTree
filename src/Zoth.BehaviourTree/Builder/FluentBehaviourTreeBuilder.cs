using System;
using Zoth.BehaviourTree.Exceptions;

namespace Zoth.BehaviourTree.Builder
{
    public class FluentBehaviourTreeBuilder<TTickData, TState> : IBehaviourTreeBuilder<TTickData, TState>
    {
        private IBehaviourTreeCompositeNode<TTickData, TState> _root;

        public IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeNode<TTickData, TState> node)
        {
            throw new BehaviourTreeBuilderFailedException("only types that derived from IBehaviourTreeComposition<TTickData, TState> can used as root");
        }

        public IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeCompositeNode<TTickData, TState> node)
        {
            if (_root != null)
                throw new BehaviourTreeBuilderFailedException("root already specified");

            _root = node ?? throw new ArgumentNullException(nameof(node));

            return new BehaviourTreeCompositeNodeBuilder<TTickData, TState>(_root);
        }

        public IBehaviourTreeBuilder<TTickData, TState> End()
        {
            return this;
        }

        public Func<TTickData, TState, BehaviourTreeState> Build()
        {
            if (_root == null)
                throw new BehaviourTreeBuilderFailedException("cant build whitout root");

            return _root.Compile();
        }
    }
}
