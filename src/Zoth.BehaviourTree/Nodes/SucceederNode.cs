﻿using System;

namespace Zoth.BehaviourTree.Nodes
{
    public class SucceederNode<TTick, TState> : DecoratorNodeBase<TTick, TState>
    {
        public SucceederNode(string name = "succeeder") : base(name)
        { }

        protected override Func<TTick, TState, BehaviourTreeState> CompileInternal()
        {
            var decoratedCompiledNode = DecoratedNode.Compile();

            return (tick, state) =>
            {
                decoratedCompiledNode(tick, state);

                return BehaviourTreeState.Success;
            };
        }
    }
}
