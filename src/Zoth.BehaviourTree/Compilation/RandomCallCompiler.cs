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

        private readonly Random _roll = new Random(64894121);

        public RandomCallCompiler(
            IEnumerable<RandomEntry<IBehaviourTreeNode<TTick, TState>>> entries,
            Func<BehaviourTreeState, bool> terminationСondition)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            if (!entries.Any())
                throw new ArgumentException(nameof(entries));

            _entries = entries;
            _terminationСondition = terminationСondition;
        }

        public Func<TTick, TState, BehaviourTreeState> Compile(bool stateful = false)
        {
            var nodes = _entries
              .Select(entry => new CompailedActionProbability(entry.Probability, entry.Entry.Compile()))
              .ToArray();

            var executedNodes = new bool[nodes.Length];

            var totalSum = (uint)nodes.Sum(entry => entry.Probability);
            var remainder = totalSum;

            Func<TTick, TState, BehaviourTreeState> runningAction = null;

            Func<TTick, TState, BehaviourTreeState> GetNextAction()
            {
                if (remainder == 0)
                {
                    return null;
                }

                var roll = GetRandom((int)remainder);

                for (int i = 0; i < nodes.Length; i++)
                {
                    if (executedNodes[i]) continue;

                    var entry = nodes[i];

                    // if probability in range and this action was not executed before
                    if (entry.Probability >= roll)
                    {
                        // if we found an acceptable entry
                        remainder -= entry.Probability;
                        executedNodes[i] = true;
                        return entry.Action;
                    }

                    roll = roll - entry.Probability;
                }

                // probably should not happens
                return null;
            }

            void Reset()
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    executedNodes[i] = false;
                }

                remainder = totalSum;
            }

            return (tick, state) =>
            {
                // if previous action not null take it if not take next
                var action = runningAction ?? GetNextAction();

                var nodeState = BehaviourTreeState.Error;

                while (action != null)
                {
                    nodeState = action(tick, state);

                    // if state running always stick with that action 
                    if (nodeState == BehaviourTreeState.Running && stateful)
                    {
                        runningAction = action;
                        return nodeState;
                    }

                    // if state error or termination condition is fulfilled and call is not stateful
                    if (nodeState == BehaviourTreeState.Running || 
                        nodeState == BehaviourTreeState.Error || 
                        _terminationСondition(nodeState))
                    {
                        break;
                    }

                    action = GetNextAction();
                }

                Reset();
                runningAction = null;

                return nodeState;
            };
        }

        private class CompailedActionProbability
        {
            public Func<TTick, TState, BehaviourTreeState> Action { get; }
            public uint Probability { get; }

            public CompailedActionProbability(uint probability, Func<TTick, TState, BehaviourTreeState> action)
            {
                Probability = probability;
                Action = action;
            }
        }

        private uint GetRandom(int max)
        {
            return (uint)_roll.Next(0, max);
        }
    }
}