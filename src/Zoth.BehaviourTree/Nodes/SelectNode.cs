using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Compilation;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    /// <summary>
    /// a selector will return a success if any of its children succeed and not process any further children. 
    /// It will process the first child, and if it fails will process the second, 
    /// and if that fails will process the third, until a success is reached, at which point it will instantly return success.
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
                _nodes, (nodeState) => nodeState != BehaviourTreeState.Success);

            return (tick, state) =>
            {

                Profiler?.LevelDown();
                Profiler?.LogExecutingAction(Name, tick);

                var func = callChainCompiler.Compile(Stateful);
                var nodeState = func(tick, state);

                Profiler?.LogExecutedAction(Name, tick, nodeState);
                Profiler?.LevelUp();

                return nodeState;
            };
        }
    }
}
