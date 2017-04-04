using System;

namespace Zoth.BehaviourTree.Builders
{
    public class BehaviourTreeNodeRandomSequenceBuilder<TTick, TState> 
    {
        private IBehaviourTreeNodeRandomSequence<TTick, TState> _parentNode;

        public BehaviourTreeNodeRandomSequenceBuilder(IBehaviourTreeNodeRandomSequence<TTick, TState> parentNode)
        {
            if (parentNode == null)
                throw new ArgumentNullException(nameof(parentNode));

            _parentNode = parentNode;
        }

        public BehaviourTreeNodeRandomSequenceBuilder<TTick, TState> Add(uint probability, IBehaviourTreeNode<TTick, TState> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _parentNode.AddNode(node, probability);

            return this;
        }

        public BehaviourTreeNodeRandomSequenceBuilder<TTick, TState> Add(
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

        public BehaviourTreeNodeRandomSequenceBuilder<TTick, TState> Add(
            uint probability,
            IBehaviourTreeNodeRandomSequence<TTick, TState> node,
            Action<BehaviourTreeNodeRandomSequenceBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.AddNode(node, probability);

            config(new BehaviourTreeNodeRandomSequenceBuilder<TTick, TState>(node));

            return this;
        }
    }
}
