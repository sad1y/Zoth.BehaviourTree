using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Tests
{
    public class SelectNodeTests
    {
        [Fact]
        public void StatefulSelectNode()
        {
            var moqAction1 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction2 = new Mock<IBehaviourTreeNode<int, int>>();

            var selectNode = new SelectNode<int, int>("test", true);

            int func1CallCount = 0, func2CallCount = 0;

            moqAction1.Setup(f => f.Compile()).Returns((tick, state) => {
                func1CallCount++;
                return BehaviourTreeState.Failure;
            });
            moqAction2.Setup(f => f.Compile()).Returns((tick, state) => {
                func2CallCount++;
                return BehaviourTreeState.Running;
            });

            selectNode.AddNode(moqAction1.Object);
            selectNode.AddNode(moqAction2.Object);

            var func = selectNode.Compile();

            var callResult1 = func(0, 0);
            var callResult2 = func(0, 0);

            Assert.Equal(BehaviourTreeState.Running, callResult1);
            Assert.Equal(BehaviourTreeState.Running, callResult2);

            Assert.Equal(1, func1CallCount);
            Assert.Equal(2, func2CallCount);
        }

        [Fact]
        public void StatelessSelectNode()
        {
            var moqAction1 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction2 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction3 = new Mock<IBehaviourTreeNode<int, int>>();

            var selectNode = new SelectNode<int, int>("test", false);

            int func1CallCount = 0, func2CallCount = 0, func3CallCount = 0;

            moqAction1.Setup(f => f.Compile()).Returns((tick, state) => {
                func1CallCount++;
                return BehaviourTreeState.Failure;
            });

            moqAction2.Setup(f => f.Compile()).Returns((tick, state) => {
                func2CallCount++;
                return BehaviourTreeState.Running;
            });

            moqAction3.Setup(f => f.Compile()).Returns((tick, state) => {
                func3CallCount++;
                return BehaviourTreeState.Success;
            });

            selectNode.AddNode(moqAction1.Object);
            selectNode.AddNode(moqAction2.Object);
            selectNode.AddNode(moqAction3.Object);

            var func = selectNode.Compile();

            var callResult1 = func(0, 0);
            var callResult2 = func(0, 0);

            Assert.Equal(BehaviourTreeState.Running, callResult1);
            Assert.Equal(BehaviourTreeState.Running, callResult2);

            Assert.Equal(2, func1CallCount);
            Assert.Equal(2, func2CallCount);
            Assert.Equal(0, func3CallCount);
        }

        [Fact]
        public void CompileWithoutSetup()
        {
            var selectNode = new SelectNode<int, int>("test", false);

            Assert.Throws<BehaviourTreeCompilationException>(() => {
                selectNode.Compile();
            });
        }

        [Theory, MemberData(nameof(Data))]
        public void VerifyStatelessExecution(IEnumerable<IBehaviourTreeNode<int, int>> nodes, BehaviourTreeState expectedState) {

            var selectNode = new SelectNode<int, int>("test", false);

            foreach (var node in nodes)
            {
                selectNode.AddNode(node);
            }

            var func = selectNode.Compile();

            var state = func(0, 0);

            Assert.Equal(expectedState, state);
        }

        [Theory, MemberData(nameof(Data))]
        public void VerifyExecution(IEnumerable<IBehaviourTreeNode<int, int>> nodes, BehaviourTreeState expectedState)
        {
            var selectNode = new SelectNode<int, int>("test", true);

            foreach (var node in nodes)
            {
                selectNode.AddNode(node);
            }

            var func = selectNode.Compile();

            var state = func(0, 0);

            Assert.Equal(expectedState, state);
        }

        public static IEnumerable<object[]> Data
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

                yield return new object[] {
                    new [] { error.Object, running.Object, success.Object, fail.Object },
                    BehaviourTreeState.Error
                };

                yield return new object[] {
                    new [] { fail.Object, success.Object, success.Object },
                    BehaviourTreeState.Success
                };

                yield return new object[] {
                    new [] { fail.Object, fail.Object , fail.Object },
                    BehaviourTreeState.Failure
                };

                yield return new object[] {
                    new [] { fail.Object, running.Object, success.Object, fail.Object },
                    BehaviourTreeState.Running
                };
            }
        }
    }
}
