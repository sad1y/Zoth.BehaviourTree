using System;
using Zoth.BehaviourTree.Builders;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Extentions
{
    public static class BehaviourTreeNodeSequenceBuilderExtentions
    {
        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Do<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, 
            string name, 
            Func<TTickData, TState, BehaviourTreeState> action)
        {
            var newNode = new ActionNode<TTickData, TState>(name, action);

            return builder.Add(newNode);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Condition<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, 
            string name, 
            Func<TTickData, TState, bool> predicate)
        {
            var newNode = new ActionNode<TTickData, TState>(name,
                (tick, state) => predicate(tick, state) ? BehaviourTreeState.Success : BehaviourTreeState.Failure);

            builder.Add(newNode);

            return builder;
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Select<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, string name,
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
            bool stateful = false)
        {
            var newNode = new SelectNode<TTickData, TState>(name, stateful);

            return builder.Add(newNode, config);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Sequence<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, string name,
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
            bool stateful = false)
        {
            var newNode = new SequenceNode<TTickData, TState>(name, stateful);

            return builder.Add(newNode, config);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> RandomSequence<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config
            )
        {
            var newNode = new RandomSequenceNode<TTickData, TState>(name);

            return builder.Add(newNode, config);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> RandomSelect<TTickData, TState>
          (this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder,
              string name,
              Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config
          )
        {
            var newNode = new RandomSelectNode<TTickData, TState>(name);

            return builder.Add(newNode, config);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Inverter<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder,
            string name,
            Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config)
        {
            var newNode = new InverterNode<TTickData, TState>(name);

            return builder.Add(newNode, config);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Succeeder<TTickData, TState>(
            this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config)
        {
            var newNode = new SucceederNode<TTickData, TState>(name);

            return builder.Add(newNode, config);
        }
    }
}
