using System;

namespace Zoth.BehaviourTree.Nodes
{
    public class InvertNode<TTick, TState> : DecoratorNodeBase<TTick, TState>
    {
        public InvertNode() : base("Invert")
        {
        }

        protected override Func<TTick, TState, BehaviourTreeState> CompileInternal()
        {
            var compiled = DecoratedNode.Compile();

            return (tick, state) => {
                var nodeState = compiled(tick, state);

                if (nodeState == BehaviourTreeState.Success)
                    return BehaviourTreeState.Failure;

                if (nodeState == BehaviourTreeState.Failure)
                    return BehaviourTreeState.Success;

                return nodeState;
            };
        }
    }
}
