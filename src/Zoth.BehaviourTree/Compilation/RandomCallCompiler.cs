using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Compilation
{
    internal class RandomCallCompiler<TTick, TState>
    {
        private readonly IEnumerable<RandomEntry<IBehaviourTreeNode<TTick, TState>>> _entries;
        private readonly Func<BehaviourTreeState, bool> _terminationСondition;

        public RandomCallCompiler(
            IEnumerable<RandomEntry<IBehaviourTreeNode<TTick, TState>>> entries,
            Func<BehaviourTreeState, bool> terminationСondition)
        {
            _entries = entries;
            _terminationСondition = terminationСondition;
        }

        public Func<TTick, TState, BehaviourTreeState> Compile()
        {
            var map = _entries.Select(f => new { Probability = (int)f.Probability, Action = f.Entry.Compile() });

            var shuffledByProbability = map
                .OrderByDescending(f => GetRandom(f.Probability))
                .Select(f => f.Action);

            return (tick, state) =>
            {
                var previousCallState = BehaviourTreeState.Error;

                foreach (var node in shuffledByProbability)
                {
                    var nodeState = node(tick, state);

                    if (
                        nodeState == BehaviourTreeState.Running ||
                        nodeState == BehaviourTreeState.Error ||
                        _terminationСondition(nodeState))
                    {
                        return nodeState;
                    }

                    previousCallState = nodeState;
                }

                return previousCallState;
            };
        }

        private static Random Rng = new Random();
        private static object syncObject = new object();

        private static int GetRandom(int max)
        {
            lock (syncObject)
            {
                return Rng.Next(max);
            }
        }
    }
}
