namespace Zoth.BehaviourTree
{
    public interface IActionProfiler<TTickData>
    {
        void LogExecutingAction(string actionName, TTickData tick);
        void LogExecutedAction(string actionName, TTickData tick, BehaviourTreeState nodeState);

        void LevelUp();
        void LevelDown();
    }
}
