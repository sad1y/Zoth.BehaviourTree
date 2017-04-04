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

        public Func<TTick, TState, BehaviourTreeState> Compile(bool stateful = false)
        {
            var rnd = new Random();

            var maxValue = 0;

            var iterator = _entries.GetEnumerator();

            var map = new Dictionary<int, Func<TTick, TState, BehaviourTreeState>>();

            while (iterator.MoveNext())
            {
                var chance = (int)iterator.Current.Probability;
                var action = iterator.Current.Entry.Compile();

                maxValue += chance > 0 ? chance : 1;
                map[maxValue] = action;
            }

            return (tick, state) =>
            {
                var roll = rnd.Next(0, maxValue);

                for (int i = 0; i < map.Count; i++)
                {
                    var entry = map.ElementAt(i);
                    if (entry.Key <= roll)
                        return entry.Value(tick, state);
                }

                return BehaviourTreeState.Error;
            };
        }
    }
}
