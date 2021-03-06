﻿using Moq;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Tests
{
    public class InverterNodeTests
    {
        [Fact]
        public void WithoutDecoratedNode()
        {
            var invert = new InverterNode<int, int>();

            Assert.Throws<BehaviourTreeException>(() =>
            {
                invert.Compile();
            });
        }

        [Fact]
        public void AddMoreThatOneNode()
        {
            var invert = new InverterNode<int, int>();

            var mockNode = new Mock<IBehaviourTreeNode<int, int>>();

            Assert.Throws<BehaviourTreeException>(() =>
            {
                invert.Decorate(mockNode.Object);
                invert.Decorate(mockNode.Object);
            });
        }

        [Theory, MemberData(nameof(ExecutionData))]
        public void ExecutionResult(IBehaviourTreeNode<int, int> node, BehaviourTreeState expectedState)
        {
            var invert = new InverterNode<int, int>();
            invert.Decorate(node);

            var func = invert.Compile();
            var state = func(0, 0);

            Assert.Equal(expectedState, state);
        }

        [Fact]
        public void ProfilerResult()
        {
            var invert = new InverterNode<int, int>();
            var profiler = new LogProfiler<int>();

            var node = new ActionNode<int, int>("test", (p1, p2)=> {
                return BehaviourTreeState.Success;
            });

            invert.Decorate(node);
            invert.Profiler = profiler;

            var func = invert.Compile();
            var state = func(0, 0);

            // Assert.Equal(expectedState, state);
        }

        public static IEnumerable<object[]> ExecutionData
        {
            get
            {
                var success = new Mock<IBehaviourTreeNode<int, int>>();
                success.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Success);

                var fail = new Mock<IBehaviourTreeNode<int, int>>();
                fail.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Failure);

                var running = new Mock<IBehaviourTreeNode<int, int>>();
                running.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Running);

                var error = new Mock<IBehaviourTreeNode<int, int>>();
                error.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Error);

                yield return new object[] { success.Object, BehaviourTreeState.Failure };
                yield return new object[] { fail.Object, BehaviourTreeState.Success };
                yield return new object[] { running.Object, BehaviourTreeState.Running };
                yield return new object[] { error.Object, BehaviourTreeState.Error };
            }
        }
    }
}
