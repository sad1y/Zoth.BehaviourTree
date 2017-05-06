using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Compilation;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Extentions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    /// <summary>
    /// A sequence will visit each child in order, starting with the first, 
    /// and when that succeeds will call the second, and so on down the list of children. 
    /// If any child fails it will immediately return failure to the parent. 
    /// If the last child in the sequence succeeds, then the sequence will return success to its parent.
    /// </summary>
    public class SequenceNode<TTickData, TState> : IBehaviourTreeNodeSequence<TTickData, TState>
    {
        private readonly IList<IBehaviourTreeNode<TTickData, TState>> _nodes = new List<IBehaviourTreeNode<TTickData, TState>>();

        public ITickProfiler<TTickData> Profiler { get; set; }

        public string Name { get; }

        public bool Stateful { get; }

        public SequenceNode(string name, bool stateful)
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
            if (!_nodes.Any())
                throw new BehaviourTreeCompilationException(ExceptionMessages.ChildShouldNotBeEmpty);

            foreach (var node in _nodes)
                node.Profiler = Profiler;

            var compiler = new SequentialCallCompiler<TTickData, TState>(
                _nodes, (nodeState) => nodeState != BehaviourTreeState.Success);

            var func = compiler.Compile(Stateful);

            return Profiler.Decorate(Name, func, true);
        }
    }
}
