namespace Zoth.BehaviourTree
{
    public interface IBehaviourTreeNodeRandom<TTickData, TState> : IBehaviourTreeNode<TTickData, TState>
    {
        void AddNode(IBehaviourTreeNode<TTickData, TState> node, uint probability);
    }
}
