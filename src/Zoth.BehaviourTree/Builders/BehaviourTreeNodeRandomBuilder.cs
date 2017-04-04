using System;

namespace Zoth.BehaviourTree.Builders
{
    public class BehaviourTreeNodeRandomBuilder<TTick, TState> 
    {
        private IBehaviourTreeNodeRandom<TTick, TState> _parentNode;

        public BehaviourTreeNodeRandomBuilder(IBehaviourTreeNodeRandom<TTick, TState> parentNode)
        {
            if (parentNode == null)
                throw new ArgumentNullException(nameof(parentNode));

            _parentNode = parentNode;
        }

        public BehaviourTreeNodeRandomBuilder<TTick, TState> Add(uint probability, IBehaviourTreeNode<TTick, TState> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _parentNode.AddNode(node, probability);

            return this;
        }

        public BehaviourTreeNodeRandomBuilder<TTick, TState> Add(
            uint probability,
            IBehaviourTreeNodeSequence<TTick, TState> node,
            Action<BehaviourTreeNodeSequenceBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node, probability);

            config(new BehaviourTreeNodeSequenceBuilder<TTick, TState>(node));

            return this;
        }

        public BehaviourTreeNodeRandomBuilder<TTick, TState> Add(
            uint probability,
            IBehaviourTreeNodeRandom<TTick, TState> node,
            Action<BehaviourTreeNodeRandomBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node, probability);

            config(new BehaviourTreeNodeRandomBuilder<TTick, TState>(node));

            return this;
        }

        public BehaviourTreeNodeRandomBuilder<TTick, TState> Add(
            uint probability,
            IBehaviourTreeNodeDecorator<TTick, TState> node,
            Action<BehaviourTreeNodeDecoratorBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node, probability);

            config(new BehaviourTreeNodeDecoratorBuilder<TTick, TState>(node));

            return this;
        }
    }
}
