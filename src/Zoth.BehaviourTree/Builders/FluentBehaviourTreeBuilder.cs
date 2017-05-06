using System;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Builders
{
    public class FluentBehaviourTreeBuilder<TTickData, TState>
    {
        private IBehaviourTreeNode<TTickData, TState> _root;
        private ITickProfiler<TTickData> _profiler;

        private void SetRootInternal(IBehaviourTreeNode<TTickData, TState> root)
        {
            if (_root != null)
                throw new BehaviourTreeBuilderException("root already specified");

            _root = root;
        }

        public FluentBehaviourTreeBuilder<TTickData, TState> Profiler(ITickProfiler<TTickData> profiler)
        {
            _profiler = profiler;

            return this;
        }

        public void Root(IBehaviourTreeNodeSequence<TTickData, TState> root,
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config)
        {
            var rootBuilder = new BehaviourTreeNodeSequenceBuilder<TTickData, TState>(root);

            config(rootBuilder);

            SetRootInternal(root);
        }

        public void Root(IBehaviourTreeNodeRandom<TTickData, TState> root,
            Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config)
        {
            var rootBuilder = new BehaviourTreeNodeRandomBuilder<TTickData, TState>(root);

            config(rootBuilder);

            SetRootInternal(root);
        }

        const string RootName = "root";

        public void Select(Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = false)
        {
            Select(RootName, config, stateful);
        }

        public void Select(string name, Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = false)
        {
            var root = new SelectNode<TTickData, TState>(name, stateful);

            Root(root, config);
        }

        public void Sequence(Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = false)
        {
            Sequence(RootName, config, stateful);
        }

        public void Sequence(string name, Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config, bool stateful = false)
        {
            var root = new SequenceNode<TTickData, TState>(name, stateful);

            Root(root, config);
        }

        public void RandomSequence(Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config, bool stateful = false)
        {
            RandomSequence(RootName, config, stateful);
        }

        public void RandomSequence(string name, Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config, bool stateful = false)
        {
            var root = new RandomSequenceNode<TTickData, TState>(name, stateful);

            Root(root, config);
        }

        public void RandomSelect(Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config, bool stateful = false)
        {
            RandomSelect(RootName, config, stateful);
        }

        public void RandomSelect(string name, Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config, bool stateful = false)
        {
            var root = new RandomSelectNode<TTickData, TState>(RootName, stateful);

            Root(root, config);
        }

        public IBehaviourTreeNode<TTickData, TState> Build()
        {
            if (_root == null)
                throw new BehaviourTreeBuilderException("cant build whitout root");

            if (_profiler != null)
                _root.Profiler = _profiler;

            return _root;
        }
    }
}