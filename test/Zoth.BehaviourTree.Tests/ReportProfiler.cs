using System;
using System.Collections.Generic;

namespace Zoth.BehaviourTree.Tests
{
    public class LogProfiler<T1> : ITickProfiler<T1>
    {
        private List<string> _log = new List<string>();

        public IEnumerable<string> Log => _log;

        public void LevelDown()
        {
            _log.Add("down");
        }

        public void LevelUp()
        {
            _log.Add("up");
        }

        public void LogExecutedAction(string actionName, T1 tick, BehaviourTreeState nodeState)
        {
            _log.Add("executed [" + actionName + "]");
        }

        public void LogExecutingAction(string actionName, T1 tick)
        {
            _log.Add("executing [" + actionName + "]");
        }
    }
}
