using System;
using System.Linq;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Compilation
{
    internal class CallChainCompiler<TTick, TState>
    {
        private Func<TTick, TState, BehaviourTreeState> _root = null;
        private Func<TTick, TState, BehaviourTreeState> _runningAction = null;
        private Func<BehaviourTreeState, bool> _nextActionCondition = null;

        private IEnumerable<IBehaviourTreeNode<TTick, TState>> _tree;

        public CallChainCompiler(
            IEnumerable<IBehaviourTreeNode<TTick, TState>> tree, 
            Func<BehaviourTreeState, bool> nextActionCondition)
        {
            _tree = tree;
            _nextActionCondition = nextActionCondition;
        }

        public Func<TTick, TState, BehaviourTreeState> Compile(bool stateful = false)
        {
            // build a sequence of calls
            foreach (var node in _tree.Reverse())
            {
                var compiledNode = node.Compile();
                _root = WrapNextCall(compiledNode, _root, _nextActionCondition);
            }

            return (tick, state) =>
            {
                var enteryPoint = _runningAction != null && stateful ? 
                    _runningAction : _root;

                return enteryPoint(tick, state);
            };
        }

        private Func<TTick, TState, BehaviourTreeState> WrapNextCall(
            Func<TTick, TState, BehaviourTreeState> action,
            Func<TTick, TState, BehaviourTreeState> nextFunc,
            Func<BehaviourTreeState, bool> invokeNextCondition)
        {
            return (tick, state) =>
            {
                var nodeState = action(tick, state);

                if(nodeState == BehaviourTreeState.Running)
                {
                    return nodeState;
                }

                if (!invokeNextCondition(nodeState))
                {
                    _runningAction = null;
                    return nodeState;
                }

                _runningAction = nextFunc;

                return nextFunc == null ? nodeState : nextFunc(tick, state);
            };
        }
    }
}
