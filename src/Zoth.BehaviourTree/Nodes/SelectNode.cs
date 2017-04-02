using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Compilation;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    /// <summary>
    /// Fallback nodes are used to find and execute the first child that does not fail. 
    /// A fallback node will return immediately with a status code of success or running when one of its children returns success or running. 
    /// The children are ticked in order of importance, from left to right.
    /// </summary>
    /// <typeparam name="TTickData"></typeparam>
    /// <typeparam name="TState"></typeparam>
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
            if (!_nodes.Any())
                throw new BehaviourTreeCompilationFailedException(ExceptionMessages.ChildShouldNotBeEmpty);

            var callChainCompiler = new CallChainCompiler<TTickData, TState>(
                _nodes, (nodeState) => nodeState == BehaviourTreeState.Failure);

            return callChainCompiler.Compile(Stateful);
        }
    }
}
