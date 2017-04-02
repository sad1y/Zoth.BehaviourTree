namespace Zoth.BehaviourTree
{
    public interface IActionProfiler<TTickData>
    {
        void LogExecutingAction(int depth, string actionName, TTickData tick);
        void LogExecutedAction(int depth, string actionName, TTickData tick, BehaviourTreeState nodeState);
    }
}
