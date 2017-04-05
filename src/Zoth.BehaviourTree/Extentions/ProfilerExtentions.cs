using System;

namespace Zoth.BehaviourTree.Extentions
{
    public static class ProfilerExtentions
    {
        public static Func<TTick, TState, BehaviourTreeState> Wrap<TTick, TState>
            (this ITickProfiler<TTick> profiler, string funcName, Func<TTick, TState, BehaviourTreeState> func, bool container = false)
        {
            if (profiler == null) return func;

            return (tick, state) =>
            {

                profiler.LogExecutingAction(funcName, tick);

                if (container)
                    profiler.LevelDown();

                var nodeState = func(tick, state);

                profiler.LogExecutedAction(funcName, tick, nodeState);

                if (container)
                    profiler.LevelUp();

                return nodeState;
            };
        }
    }
}
