namespace Zoth.BehaviourTree
{
    public interface IBehaviourTreeNodeRandomSequence<TTickData, TState> : IBehaviourTreeNode<TTickData, TState>
    {
        void AddNode(IBehaviourTreeNode<TTickData, TState> node, uint probability);
    }
}
