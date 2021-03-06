﻿using System;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Extentions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public abstract class DecoratorNodeBase<TTickData, TState> : IBehaviourTreeNodeDecorator<TTickData, TState>
    {
        public string Name { get; }

        public ITickProfiler<TTickData> Profiler { get; set; }

        protected IBehaviourTreeNode<TTickData, TState> DecoratedNode;

        public DecoratorNodeBase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        protected abstract Func<TTickData, TState, BehaviourTreeState> CompileInternal();

        public Func<TTickData, TState, BehaviourTreeState> Compile()
        {
            if (DecoratedNode == null)
                throw new BehaviourTreeException(ExceptionMessages.DecoratedNodeNotProvided);

            if (DecoratedNode != null)
                DecoratedNode.Profiler = Profiler;

            var func = CompileInternal();

            return Profiler.Decorate(Name, func, container: true);
        }

        public void Decorate(IBehaviourTreeNode<TTickData, TState> node)
        {
            if (DecoratedNode != null)
                throw new BehaviourTreeException(ExceptionMessages.СantDecorateMoreThanOneNode);

            DecoratedNode = node ?? throw new ArgumentNullException(nameof(node));
        }
    }
}
