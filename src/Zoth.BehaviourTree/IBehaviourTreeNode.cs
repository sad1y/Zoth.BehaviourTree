using System;

namespace Zoth.BehaviourTree
{
    public interface IBehaviourTreeNode<TTickData, TState>
    {
        string Name { get; }

        IActionProfiler<TTickData> Profiler { get; set; }

        Func<TTickData, TState, BehaviourTreeState> Compile();
    }
}
