using System;

namespace Zoth.BehaviourTree.Builders
{
    public class BehaviourTreeNodeSequenceBuilder<TTickData, TState>
    {
        private IBehaviourTreeNodeSequence<TTickData, TState> _parentNode;

        public BehaviourTreeNodeSequenceBuilder(IBehaviourTreeNodeSequence<TTickData, TState> parentNode)
        {
            if(parentNode == null)
                throw new ArgumentNullException(nameof(parentNode));

            _parentNode = parentNode;
        }

        public BehaviourTreeNodeSequenceBuilder<TTickData, TState> Add(IBehaviourTreeNode<TTickData, TState> node)
        {
            if(node == null)
                throw new ArgumentNullException(nameof(node));

            _parentNode.AddNode(node);

            return this;
        }

        public BehaviourTreeNodeSequenceBuilder<TTickData, TState> Add(
            IBehaviourTreeNodeSequence<TTickData, TState> node, 
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if(config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node);

            config(new BehaviourTreeNodeSequenceBuilder<TTickData, TState>(node));

            return this;
        }

        public BehaviourTreeNodeSequenceBuilder<TTickData, TState> Add(
            IBehaviourTreeNodeRandom<TTickData, TState> node,
            Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node);

            config(new BehaviourTreeNodeRandomBuilder<TTickData, TState>(node));

            return this;
        }

        public BehaviourTreeNodeSequenceBuilder<TTickData, TState> Add(
            IBehaviourTreeNodeDecorator<TTickData, TState> node,
            Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node);

            config(new BehaviourTreeNodeDecoratorBuilder<TTickData, TState>(node));

            return this;
        }
    }
}