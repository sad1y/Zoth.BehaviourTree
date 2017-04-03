using System;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public abstract class DecoratorNodeBase<TTickData, TState> : IBehaviourTreeCompositeNode<TTickData, TState>
    {
        public string Name { get; }

        public IActionProfiler<TTickData> Profiler { get; set; }

        protected IBehaviourTreeNode<TTickData, TState> DecoratedNode;

        public DecoratorNodeBase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public void AddNode(IBehaviourTreeNode<TTickData, TState> node)
        {
            if (DecoratedNode != null)
                throw new BehaviourTreeException(ExceptionMessages.СantDecorateMoreThanOneNode);

            DecoratedNode = node ?? throw new ArgumentNullException(nameof(node));
        }

        protected abstract Func<TTickData, TState, BehaviourTreeState> CompileInternal();

        public Func<TTickData, TState, BehaviourTreeState> Compile()
        {
            if (DecoratedNode == null)
                throw new BehaviourTreeException(ExceptionMessages.DecoratedNodeNotProvided);

            var compiled = CompileInternal();

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
