namespace Zoth.BehaviourTree.Builder
{
    public interface IBehaviourTreeBuilder<TTickData, TState>
    {
        IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeNode<TTickData, TState> node);

        IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeCompositeNode<TTickData, TState> node);

        IBehaviourTreeBuilder<TTickData, TState> End();
    }
}
