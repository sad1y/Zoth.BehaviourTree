using System;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public abstract class DecoratorNodeBase<TTickData, TState> : IBehaviourTreeCompositeNode<TTickData, TState>
    {
        public string Name { get; }

        public IActionProfiler<TTickData> Profiler { get; set; }

        protected IBehaviourTreeNode<TTickData, TState> _wrappedNode;

        public DecoratorNodeBase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public void AddNode(IBehaviourTreeNode<TTickData, TState> node)
        {
            if (_wrappedNode != null)
                throw new BehaviourTreeException(ExceptionMessages.СantDecorateMoreThanOneNode);

            _wrappedNode = node ?? throw new ArgumentNullException(nameof(node));
        }

        public Func<TTickData, TState, BehaviourTreeState> Compile()
        {
            if (_wrappedNode == null)
                throw new BehaviourTreeException(ExceptionMessages.DecoratedNodeNotProvided);

            var compiled = _wrappedNode.Compile();

            return Profiler == null ? compiled :
                (tick, state) =>
                {
                    Profiler?.LogExecutingAction(Name, tick);

                    var nodeState = compiled(tick, state);

                    Profiler?.LogExecutedAction(Name, tick, nodeState);

                    return nodeState;
                };
        }
    }
}
