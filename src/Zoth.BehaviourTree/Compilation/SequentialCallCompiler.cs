using System;
using System.Linq;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Compilation
{
    internal class SequentialCallCompiler<TTick, TState>
    {
        private Func<BehaviourTreeState, bool> _terminationCondition = null;

        private IEnumerable<IBehaviourTreeNode<TTick, TState>> _nodes;

        public SequentialCallCompiler(
            IEnumerable<IBehaviourTreeNode<TTick, TState>> nodes,
            Func<BehaviourTreeState, bool> terminationCondition)
        {
            _nodes = nodes;
            _terminationCondition = terminationCondition;
        }

        public Func<TTick, TState, BehaviourTreeState> Compile(bool stateful = false)
        {
            var pointer = 0;

            var callSequence = _nodes
                .Select(node => node.Compile())
                .ToArray();

            return (tick, state) =>
            {
                var nodeState = BehaviourTreeState.Error;

                while(pointer < callSequence.Length)
                {
                    var action = callSequence[pointer];

                    nodeState = action(tick, state);

                    if (nodeState == BehaviourTreeState.Running && stateful)
                    {
                        return nodeState;
                    }

                    if (nodeState == BehaviourTreeState.Running ||
                        nodeState == BehaviourTreeState.Error ||
                        _terminationCondition(nodeState))
                    {
                        break;
                    }

                    pointer++;
                }

                pointer = 0;

                return nodeState;
            };
        }
    }
}
