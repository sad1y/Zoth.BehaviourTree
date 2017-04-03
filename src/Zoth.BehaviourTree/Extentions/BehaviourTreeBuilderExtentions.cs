using System;
using Zoth.BehaviourTree.Builders;
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

        public static IBehaviourTreeBuilder<TTickData, TState> Condition<TTickData, TState>
            (this IBehaviourTreeBuilder<TTickData, TState> builder, string name, Func<TTickData, TState, bool> predicate)
        {
            var newNode = new ActionNode<TTickData, TState>(name, 
                (tick, state) => predicate(tick, state) ? BehaviourTreeState.Success : BehaviourTreeState.Failure);

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

        public static IBehaviourTreeBuilder<TTickData, TState> Sequence<TTickData, TState>
           (this IBehaviourTreeBuilder<TTickData, TState> builder, string name, bool stateful = false)
        {
            var newNode = new SequenceNode<TTickData, TState>(name, stateful);

            builder.Add(newNode);

            return builder;
        }

        public static IBehaviourTreeBuilder<TTickData, TState> Succeeder<TTickData, TState>
           (this IBehaviourTreeBuilder<TTickData, TState> builder)
        {
            var newNode = new SucceederNode<TTickData, TState>();

            builder.Add(newNode);

            return builder;
        }

        public static IBehaviourTreeBuilder<TTickData, TState> Inverter<TTickData, TState>
           (this IBehaviourTreeBuilder<TTickData, TState> builder)
        {
            var newNode = new InverterNode<TTickData, TState>();

            builder.Add(newNode);

            return builder;
        }
    }
}
