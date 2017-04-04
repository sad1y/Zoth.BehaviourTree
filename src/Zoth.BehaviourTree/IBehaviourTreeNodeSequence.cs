namespace Zoth.BehaviourTree
{
    public interface IBehaviourTreeNodeSequence<TTickData, TState> : IBehaviourTreeNode<TTickData, TState>
    {
        void AddNode(IBehaviourTreeNode<TTickData, TState> node);
    }
}
