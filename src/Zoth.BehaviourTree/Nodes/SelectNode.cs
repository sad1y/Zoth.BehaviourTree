using System;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Nodes
{
    public class SelectNode<TTickData, TState> : IBehaviourTreeCompositeNode<TTickData, TState>
    {
        private readonly IList<IBehaviourTreeNode<TTickData, TState>> _nodes = new List<IBehaviourTreeNode<TTickData, TState>>();

        public string Name { get; }
        public bool Stateful { get; }
        public IActionProfiler<TTickData> Profiler { get; set; }

        public SelectNode(string name, bool stateful)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Stateful = stateful;
        }

        public void AddNode(IBehaviourTreeNode<TTickData, TState> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _nodes.Add(node);
        }

        public Func<TTickData, TState, BehaviourTreeState> Compile()
        {
            throw new NotImplementedException();
        }
    }
}
