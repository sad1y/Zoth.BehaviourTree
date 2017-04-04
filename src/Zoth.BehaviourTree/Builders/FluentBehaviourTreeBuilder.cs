using System;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Builders
{
    public class FluentBehaviourTreeBuilder<TTickData, TState>
    {
        private IBehaviourTreeNode<TTickData, TState> _root;

        private void SetRootInternal(IBehaviourTreeNode<TTickData, TState> root)
        {
            if (_root != null)
                throw new BehaviourTreeBuilderException("root already specified");

            _root = root;
        }

        public void Root(IBehaviourTreeNodeSequence<TTickData, TState> root, 
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config)
        {
            var rootBuilder = new BehaviourTreeNodeSequenceBuilder<TTickData, TState>(root);

            config(rootBuilder);

            SetRootInternal(root);
        }

        public void Root(IBehaviourTreeNodeRandomSequence<TTickData, TState> root,
            Action<BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState>> config)
        {
            var rootBuilder = new BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState>(root);

            config(rootBuilder);

            SetRootInternal(root);
        }

        //public void Root(IBehaviourTreeNodeDecorator<TTickData, TState> root,
        //    Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config)
        //{
        //    var rootBuilder = new BehaviourTreeNodeDecoratorBuilder<TTickData, TState>(root);

        //    config(rootBuilder);

        //    SetRootInternal(root);
        //}

        const string RootName = "root";

        public void Select(Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = true)
        {
            var root = new SelectNode<TTickData, TState>(RootName, stateful);

            Root(root, config);
        }

        public void Sequence(Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = true)
        {
            var root = new SequenceNode<TTickData, TState>(RootName, stateful);

            Root(root, config);
        }

        public void RandomSequence(Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = true)
        {
            var root = new SequenceNode<TTickData, TState>(RootName, stateful);

            Root(root, config);
        }

        public void RandomSelect(Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = true)
        {
            var root = new SequenceNode<TTickData, TState>(RootName, stateful);

            Root(root, config);
        }

        public Func<TTickData, TState, BehaviourTreeState> Build()
        {
            if (_root == null)
                throw new BehaviourTreeBuilderException("cant build whitout root");

            return _root.Compile();
        }
    }
}
