using System;

namespace Zoth.BehaviourTree.Builders
{
    public class BehaviourTreeNodeDecoratorBuilder<TTick, TState>
    {
        private IBehaviourTreeNodeDecorator<TTick, TState> _parentNode;

        public BehaviourTreeNodeDecoratorBuilder(IBehaviourTreeNodeDecorator<TTick, TState> parentNode)
        {
            if (parentNode == null)
                throw new ArgumentNullException(nameof(parentNode));

            _parentNode = parentNode;
        }

        public void Decorate(IBehaviourTreeNode<TTick, TState> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _parentNode.Decorate(node);
        }

        public void Decorate(
            IBehaviourTreeNodeSequence<TTick, TState> node,
            Action<BehaviourTreeNodeSequenceBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.Decorate(node);

            config(new BehaviourTreeNodeSequenceBuilder<TTick, TState>(node));
        }

        public void Decorate(
            IBehaviourTreeNodeRandom<TTick, TState> node,
            Action<BehaviourTreeNodeRandomBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.Decorate(node);

            config(new BehaviourTreeNodeRandomBuilder<TTick, TState>(node));
        }

        public void Decorate(
            IBehaviourTreeNodeDecorator<TTick, TState> node,
            Action<BehaviourTreeNodeDecoratorBuilder<TTick, TState>> config)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _parentNode.Decorate(node);

            config(new BehaviourTreeNodeDecoratorBuilder<TTick, TState>(node));
        }
    }
}
