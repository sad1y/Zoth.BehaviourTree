using System;
using Zoth.BehaviourTree.Builders;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Extentions
{
    public static class BehaviourTreeNodeBuilderExtentions
    {
        #region sequence 
        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Do<TTickData, TState>
            (this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, string name, Func<TTickData, TState, BehaviourTreeState> action)
        {
            var newNode = new ActionNode<TTickData, TState>(name, action);

            return builder.Add(newNode);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Condition<TTickData, TState>
            (this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, string name, Func<TTickData, TState, bool> predicate)
        {
            var newNode = new ActionNode<TTickData, TState>(name, 
                (tick, state) => predicate(tick, state) ? BehaviourTreeState.Success : BehaviourTreeState.Failure);

            builder.Add(newNode);

            return builder;
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Select<TTickData, TState>
            (this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, string name,
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
            bool stateful = false)
        {
            var newNode = new SelectNode<TTickData, TState>(name, stateful);

            return builder.Add(newNode, config);
        }

        public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Sequence<TTickData, TState>
           (this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder, string name,
            Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
            bool stateful = false)
        {
            var newNode = new SequenceNode<TTickData, TState>(name, stateful);

            return builder.Add(newNode, config);
        }

        #endregion

        #region random
        public static BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> Do<TTickData, TState>
            (this BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> builder, 
                string name, 
                uint probability, 
                Func<TTickData, TState, BehaviourTreeState> action
            )
        {
            var newNode = new ActionNode<TTickData, TState>(name, action);

            return builder.Add(probability, newNode);
        }

        public static BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> Condition<TTickData, TState>
            (this BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> builder, 
            string name,
            uint probability,
            Func<TTickData, TState, bool> predicate)
        {
            return builder.Do(name, probability,
                (tick, state) => predicate(tick, state) ? BehaviourTreeState.Success : BehaviourTreeState.Failure);
        }

        public static BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> Select<TTickData, TState>
            (this BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> builder,
                string name,
                uint probability,
                Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new SelectNode<TTickData, TState>(name, stateful);

            return builder.Add(probability, newNode, config);
        }

        public static BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> Sequence<TTickData, TState>
           (this BehaviourTreeNodeRandomSequenceBuilder<TTickData, TState> builder,
                string name,
                uint probability,
                Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new SequenceNode<TTickData, TState>(name, stateful);

            return builder.Add(probability, newNode);
        }

#endregion

        //public static BehaviourTreeNodeSequenceBuilder<TTickData, TState> Succeeder<TTickData, TState>
        //   (this BehaviourTreeNodeSequenceBuilder<TTickData, TState> builder)
        //{
        //    var newNode = new SucceederNode<TTickData, TState>();

        //    builder.Add(newNode);

        //    return builder;
        //}

        //public static IBehaviourTreeBuilder<TTickData, TState> Inverter<TTickData, TState>
        //   (this IBehaviourTreeBuilder<TTickData, TState> builder)
        //{
        //    var newNode = new InverterNode<TTickData, TState>();

        //    builder.Add(newNode);

        //    return builder;
        //}
    }
}
