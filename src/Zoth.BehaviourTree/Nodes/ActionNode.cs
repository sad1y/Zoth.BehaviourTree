using System;
using Zoth.BehaviourTree.Extentions;

namespace Zoth.BehaviourTree.Nodes
{
    public class ActionNode<TTickData, TState> : IBehaviourTreeNode<TTickData, TState>
    {
        private Func<TTickData, TState, BehaviourTreeState> _action;

        public string Name { get; }

        public ITickProfiler<TTickData> Profiler { get; set; }

        public ActionNode(string name, Func<TTickData, TState, BehaviourTreeState> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);

            Name = name;
        }

        public Func<TTickData, TState, BehaviourTreeState> Compile()
        {
            return Profiler.Wrap(Name, _action);
        }
    }
}
