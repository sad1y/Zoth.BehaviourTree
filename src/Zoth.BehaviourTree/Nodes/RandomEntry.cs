using System;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public class RandomEntry<TEntry>
    {
        public uint Probability { get; }
        public TEntry Entry { get; }

        public RandomEntry(uint probability, TEntry entry)
        {
            if (probability == 0)
                throw new ArgumentException(ExceptionMessages.RandomEntryProbabilityEqualZero);

            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            Probability = probability;
            Entry = entry;
        }
    }
}
