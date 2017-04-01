namespace Zoth.BehaviourTree
{
    public interface IBehaviourTreeCompositeNode<TTickData, TState> : IBehaviourTreeNode<TTickData, TState>
    {
        void AddNode(IBehaviourTreeNode<TTickData, TState> node);
    }
}
