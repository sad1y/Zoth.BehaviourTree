namespace Zoth.BehaviourTree.Builders
{
    public interface IBehaviourTreeBuilder<TTickData, TState>
    {
        IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeNode<TTickData, TState> node);

        IBehaviourTreeBuilder<TTickData, TState> Add(IBehaviourTreeNodeSequence<TTickData, TState> node);
        // RandomSequenceBehaviourTreeBuilder<TTickData, TState, TParentBuilder> AddRandom(IBehaviourTreeCompositeNode<TTickData, TState> node);

        // IBehaviourTreeBuilder<TTickData, TState> End();
    }
}
