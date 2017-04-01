namespace Zoth.BehaviourTree
{
    public interface IActionProfiler<TTickData>
    {
        void LogExecutingAction(int depth, string actionName, TTickData data);
        void LogExecutedAction(int depth, string actionName, TTickData data, BehaviourTreeState status);
    }
}
