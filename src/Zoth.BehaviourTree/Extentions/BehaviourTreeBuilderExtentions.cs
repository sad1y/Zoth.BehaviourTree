using System;
using Zoth.BehaviourTree.Builder;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Extentions
{
    public static class BehaviourTreeCompositeNodeBuilderExtentions
    {
        public static IBehaviourTreeBuilder<TTickData, TState> Do<TTickData, TState>
            (this IBehaviourTreeBuilder<TTickData, TState> builder, string name, Func<TTickData, TState, BehaviourTreeState> action)
        {
            var newNode = new ActionNode<TTickData, TState>(name, action);

            builder.Add(newNode);

            return builder;
        }

        public static IBehaviourTreeBuilder<TTickData, TState> Select<TTickData, TState>
            (this IBehaviourTreeBuilder<TTickData, TState> builder, string name, bool stateful = false)
        {
            var newNode = new SelectNode<TTickData, TState>(name, stateful);

            builder.Add(newNode);

            return builder;
        }
    }
}
