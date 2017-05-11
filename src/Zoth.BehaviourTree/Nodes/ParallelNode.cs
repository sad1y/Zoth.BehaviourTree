using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Compilation;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Extentions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public class ParallelNode<TTickData, TState> : IBehaviourTreeNode<TTickData, TState>
    {


        /*
         for i = 1 to N:
state_i = Tick(child[i])

if nSucc(state) >= S:
return SUCCESS
else if nFail(state) >= F:
return FAILURE
else
return RUNNING
         */
        private readonly IList<IBehaviourTreeNode<TTickData, TState>> _nodes = new List<IBehaviourTreeNode<TTickData, TState>>();

        public ITickProfiler<TTickData> Profiler { get; set; }

        public string Name { get; }

        public ParallelNode(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
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
                _nodes, (nodeState) => nodeState == BehaviourTreeState.Success);

            var func = compiler.Compile(false);

            return Profiler.Decorate(Name, func, true);
        }
    }
}
