﻿using System;
using Zoth.BehaviourTree.Builders;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Extentions
{
    public static class BehaviourTreeNodeRandomBuilderExtentions
    {
        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> Do<TTickData, TState>
            (this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
                string name,
                uint probability,
                Func<TTickData, TState, BehaviourTreeState> action
            )
        {
            var newNode = new ActionNode<TTickData, TState>(name, action);

            return builder.Add(probability, newNode);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> Condition<TTickData, TState>
            (this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
            string name,
            uint probability,
            Func<TTickData, TState, bool> predicate)
        {
            return builder.Do(name, probability,
                (tick, state) => predicate(tick, state) ? BehaviourTreeState.Success : BehaviourTreeState.Failure);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> Select<TTickData, TState>
            (this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
                string name,
                uint probability,
                Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new SelectNode<TTickData, TState>(name, stateful);

            return builder.Add(probability, newNode, config);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> Sequence<TTickData, TState>
           (this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
                string name,
                uint probability,
                Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new SequenceNode<TTickData, TState>(name, stateful);

            return builder.Add(probability, newNode, config);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> RandomSequence<TTickData, TState>(
            this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
                string name,
                uint probability,
                Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config
            )
        {
            var newNode = new RandomSequenceNode<TTickData, TState>(name);

            return builder.Add(probability, newNode, config);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> RandomSelect<TTickData, TState>
          (this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
              string name,
              uint probability,
              Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config
          )
        {
            var newNode = new RandomSelectNode<TTickData, TState>(name);

            return builder.Add(probability, newNode, config);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> Inventer<TTickData, TState>(
            this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
            uint probability,
            Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config)
        {
            var newNode = new InverterNode<TTickData, TState>();

            return builder.Add(probability, newNode, config);
        }

        public static BehaviourTreeNodeRandomBuilder<TTickData, TState> Succeeder<TTickData, TState>(
            this BehaviourTreeNodeRandomBuilder<TTickData, TState> builder,
            uint probability,
            Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config)
        {
            var newNode = new SucceederNode<TTickData, TState>();

            return builder.Add(probability, newNode, config);
        }
    }
}