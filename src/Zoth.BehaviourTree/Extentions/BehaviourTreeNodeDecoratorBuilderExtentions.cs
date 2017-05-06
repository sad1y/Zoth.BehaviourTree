using System;
using Zoth.BehaviourTree.Builders;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Extentions
{
    public static class BehaviourTreeNodeDecoratorBuilderExtentions
    {
        public static void Do<TTickData, TState>
           (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
                string name,
                Func<TTickData, TState, BehaviourTreeState> action
            )
        {
            var newNode = new ActionNode<TTickData, TState>(name, action);

            builder.Decorate(newNode);
        }

        public static void Condition<TTickData, TState>
            (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
            string name,
            Func<TTickData, TState, bool> predicate)
        {
            builder.Do(name,
                (tick, state) => predicate(tick, state) ? BehaviourTreeState.Success : BehaviourTreeState.Failure);
        }

        public static void Select<TTickData, TState>
            (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new SelectNode<TTickData, TState>(name, stateful);

            builder.Decorate(newNode, config);
        }

        public static void Sequence<TTickData, TState>
           (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeSequenceBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new SequenceNode<TTickData, TState>(name, stateful);

            builder.Decorate(newNode, config);
        }

        public static void RandomSequence<TTickData, TState>
           (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config,
                bool stateful = false
            )
        {
            var newNode = new RandomSequenceNode<TTickData, TState>(name, stateful);

            builder.Decorate(newNode, config);
        }

        public static void RandomSelect<TTickData, TState>
          (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
              string name,
              Action<BehaviourTreeNodeRandomBuilder<TTickData, TState>> config,
                bool stateful = false
          )
        {
            var newNode = new RandomSelectNode<TTickData, TState>(name, stateful);

            builder.Decorate(newNode, config);
        }

        public static void Inverter<TTickData, TState>
           (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config
            )
        {
            var newNode = new InverterNode<TTickData, TState>(name);

            builder.Decorate(newNode, config);
        }

        public static void Succeeder<TTickData, TState>
           (this BehaviourTreeNodeDecoratorBuilder<TTickData, TState> builder,
                string name,
                Action<BehaviourTreeNodeDecoratorBuilder<TTickData, TState>> config
            )
        {
            var newNode = new SucceederNode<TTickData, TState>(name);

            builder.Decorate(newNode, config);
        }
    }
}
