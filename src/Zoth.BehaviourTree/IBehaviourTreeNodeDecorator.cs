namespace Zoth.BehaviourTree
{
    public interface IBehaviourTreeNodeDecorator<TTick, TState> : 
        IBehaviourTreeNode<TTick, TState>
    {
        void Decorate(IBehaviourTreeNode<TTick, TState> node);
    }
}
