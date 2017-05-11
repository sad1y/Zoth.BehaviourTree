using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Extentions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public class ParallelNode<TTickData, TState> : IBehaviourTreeNodeSequence<TTickData, TState>
    {

        private readonly IList<IBehaviourTreeNode<TTickData, TState>> _nodes = new List<IBehaviourTreeNode<TTickData, TState>>();
        private readonly int _failureCount;
        private readonly int _successCount;

        public ITickProfiler<TTickData> Profiler { get; set; }

        public string Name { get; }

        public ParallelNode(string name, int successCount, int failureCount)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _successCount = successCount;
            _failureCount = failureCount;

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
            if (_nodes.Count < 2)
                throw new BehaviourTreeCompilationException(ExceptionMessages.ShouldContainsAtLeastTwoNode);

            foreach (var node in _nodes)
            {
                node.Profiler = Profiler;
            }

            var compiledNodes = _nodes
                .Select(node => node.Compile())
                .ToList();

            Func<TTickData, TState, BehaviourTreeState> func = (tick, state) =>
            {
                var failureCount = 0;
                var successCount = 0;

                for (int i = 0; i < compiledNodes.Count; i++)
                {
                    var action = compiledNodes[i];

                    var nodeState = action(tick, state);

                    if (nodeState == BehaviourTreeState.Error) return BehaviourTreeState.Error;

                    switch (nodeState)
                    {
                        case BehaviourTreeState.Success:
                            {
                                successCount++;
                                break;
                            }

                        case BehaviourTreeState.Failure:
                            {
                                failureCount++;
                                break;
                            }
                        default:
                            break;
                    }

                    if(successCount >= _successCount)
                    {
                        return BehaviourTreeState.Success;
                    }

                    if(failureCount >= _failureCount)
                    {
                        return BehaviourTreeState.Failure;
                    }
                }

                return BehaviourTreeState.Running;
            };


            return Profiler.Decorate(Name, func, true);
        }
    }
}
