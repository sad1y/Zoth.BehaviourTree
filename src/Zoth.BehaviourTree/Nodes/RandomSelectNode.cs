﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zoth.BehaviourTree.Compilation;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Extentions;
using Zoth.BehaviourTree.Resources;

namespace Zoth.BehaviourTree.Nodes
{
    public class RandomSelectNode<TTickData, TState> : IBehaviourTreeNodeRandomSequence<TTickData, TState>
    {
        private readonly IList<RandomEntry<IBehaviourTreeNode<TTickData, TState>>> _nodes
            = new List<RandomEntry<IBehaviourTreeNode<TTickData, TState>>>();

        public string Name { get; }
        public IActionProfiler<TTickData> Profiler { get; set; }

        public RandomSelectNode(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public void AddNode(IBehaviourTreeNode<TTickData, TState> node, uint probality)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _nodes.Add(new RandomEntry<IBehaviourTreeNode<TTickData, TState>>(probality, node));
        }

        public Func<TTickData, TState, BehaviourTreeState> Compile()
        {
            if (!_nodes.Any())
                throw new BehaviourTreeCompilationException(ExceptionMessages.ChildShouldNotBeEmpty);

            var compiler = new RandomCallCompiler<TTickData, TState>(_nodes.AsEnumerable(),
                (nodeState) => nodeState == BehaviourTreeState.Success);

            var func = compiler.Compile();

            return Profiler.Wrap(Name, func, true);
        }
    }
}
