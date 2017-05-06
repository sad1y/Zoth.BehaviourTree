using System;
using System.Linq;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Compilation
{
    [Obsolete("use SequentialCallCompiler instead")]
    internal class WrapCallCompiler<TTick, TState>
    {
        private Func<TTick, TState, BehaviourTreeState> _root = null;
        private Func<TTick, TState, BehaviourTreeState> _runningAction = null;
        private Func<BehaviourTreeState, bool> _terminationCondition = null;

        private IEnumerable<IBehaviourTreeNode<TTick, TState>> _tree;

        public WrapCallCompiler(
            IEnumerable<IBehaviourTreeNode<TTick, TState>> tree, 
            Func<BehaviourTreeState, bool> terminationCondition)
        {
            _tree = tree;
            _terminationCondition = terminationCondition;
        }

        public Func<TTick, TState, BehaviourTreeState> Compile(bool stateful = false)
        {
            // build a sequence of calls
            foreach (var node in _tree.Reverse())
            {
                var compiledNode = node.Compile();
                _root = WrapNextCall(compiledNode, _root, _terminationCondition);
            }

            return (tick, state) =>
            {
                var entryPoint = _runningAction != null && stateful ? 
                    _runningAction : _root;

                return entryPoint(tick, state);
            };
        }

        private Func<TTick, TState, BehaviourTreeState> WrapNextCall(
            Func<TTick, TState, BehaviourTreeState> action,
            Func<TTick, TState, BehaviourTreeState> nextFunc,
            Func<BehaviourTreeState, bool> terminationCondition)
        {
            return (tick, state) =>
            {
                var nodeState = action(tick, state);

                if(nodeState == BehaviourTreeState.Running)
                {
                    return nodeState;
                }

                if (nodeState == BehaviourTreeState.Error ||
                    terminationCondition(nodeState))
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
