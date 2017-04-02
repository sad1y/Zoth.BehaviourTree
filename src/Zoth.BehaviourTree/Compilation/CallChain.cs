using System;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Compilation
{
    internal class CallChain<TTick, TState>
    {
        private Func<TTick, TState, BehaviourTreeState> _root = null;
        private Func<TTick, TState, BehaviourTreeState> _runningAction = null;
        private Func<BehaviourTreeState, bool> _nextActionCondition = null;

        private readonly bool _stateful;
        private IEnumerable<IBehaviourTreeNode<TTick, TState>> _tree;

        public CallChain(
            IEnumerable<IBehaviourTreeNode<TTick, TState>> tree, 
            Func<BehaviourTreeState, bool> nextActionCondition,
            bool stateful = false)
        {
            _tree = tree;
            _nextActionCondition = nextActionCondition;
            _stateful = stateful;
        }

        public Func<TTick, TState, BehaviourTreeState> Compile()
        {
            // compile all of it
            var compiledNodes = new List<Func<TTick, TState, BehaviourTreeState>>();

            foreach (var node in _tree)
            {
                compiledNodes.Add(node.Compile());
            }

            compiledNodes.Reverse();

            // build a sequence of calls
            foreach (var compiledNode in compiledNodes)
            {
                _root = WrapNextCall(compiledNode, _root, _nextActionCondition, _stateful);
            }

            return (tick, state) =>
            {
                return (_runningAction ?? _root).Invoke(tick, state);
            };
        }

        private Func<TTick, TState, BehaviourTreeState> WrapNextCall(
            Func<TTick, TState, BehaviourTreeState> action,
            Func<TTick, TState, BehaviourTreeState> nextFunc,
            Func<BehaviourTreeState, bool> invokeNextCondition, bool stateful = false)
        {
            return (tick, state) =>
            {
                var nodeState = action(tick, state);

                if(_stateful && nodeState == BehaviourTreeState.Running)
                {
                    return nodeState;
                }

                if (!invokeNextCondition(nodeState)) return nodeState;

                _runningAction = nextFunc;

                return nextFunc == null ? nodeState : nextFunc(tick, state);
            };
        }
    }
}
