using System;
using Zoth.BehaviourTree.Exceptions;

namespace Zoth.BehaviourTree.Builders
{
    public class FluentBehaviourTreeBuilder<TTickData, TState>
    {
        private BehaviourTreeNodeSequenceBuilder<TTickData, TState> _root;

        //public IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeNode<TTickData, TState> node)
        //{
        //    throw new BehaviourTreeBuilderException("only types that derived from IBehaviourTreeComposition<TTickData, TState> can used as root");
        //}

        public void Root(
            BehaviourTreeNodeSequenceBuilder<TTickData, TState> node,
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config)
        {
            if (_root != null)
                throw new BehaviourTreeBuilderException("root already specified");

            _root = node ?? throw new ArgumentNullException(nameof(node));

            config( new BehaviourTreeNodeSequenceBuilder<TTickData, TState>(_root) );
        }

        public Func<TTickData, TState, BehaviourTreeState> Build()
        {
            if (_root == null)
                throw new BehaviourTreeBuilderException("cant build whitout root");

            return _root.Compile();
        }
    }
}
