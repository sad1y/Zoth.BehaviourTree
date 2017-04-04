using Moq;
using System.Collections.Generic;
using Xunit;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Tests
{
    public class SequenceNodeTests
    {
        [Fact]
        public void StatefulSelectNode()
        {
            var moqAction1 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction2 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction3 = new Mock<IBehaviourTreeNode<int, int>>();

            var selectNode = new SequenceNode<int, int>("test", true);

            int func1CallCount = 0, func2CallCount = 0, func3CallCount = 0;

            moqAction1.Setup(f => f.Compile()).Returns((tick, state) => {
                func1CallCount++;
                return BehaviourTreeState.Success;
            });

            moqAction2.Setup(f => f.Compile()).Returns((tick, state) => {
                func2CallCount++;
                return BehaviourTreeState.Running;
            });

            moqAction3.Setup(f => f.Compile()).Returns((tick, state) => {
                func3CallCount++;
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
            Assert.Equal(0, func3CallCount);
        }

        [Fact]
        public void StatelessSelectNode()
        {
            var moqAction1 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction2 = new Mock<IBehaviourTreeNode<int, int>>();
            var moqAction3 = new Mock<IBehaviourTreeNode<int, int>>();

            var selectNode = new SequenceNode<int, int>("test", false);

            int func1CallCount = 0, func2CallCount = 0, func3CallCount = 0;

            moqAction1.Setup(f => f.Compile()).Returns((tick, state) => {
                func1CallCount++;
                return BehaviourTreeState.Success;
            });

            moqAction2.Setup(f => f.Compile()).Returns((tick, state) => {
                func2CallCount++;
                return BehaviourTreeState.Failure;
            });

            moqAction3.Setup(f => f.Compile()).Returns((tick, state) => {
                func3CallCount++;
                return BehaviourTreeState.Running;
            });

            selectNode.AddNode(moqAction1.Object);
            selectNode.AddNode(moqAction2.Object);
            selectNode.AddNode(moqAction3.Object);

            var func = selectNode.Compile();

            var callResult1 = func(0, 0);
            var callResult2 = func(0, 0);

            Assert.Equal(BehaviourTreeState.Failure, callResult1);
            Assert.Equal(BehaviourTreeState.Failure, callResult2);

            Assert.Equal(2, func1CallCount);
            Assert.Equal(2, func2CallCount);
            Assert.Equal(0, func3CallCount);
        }

        [Theory, MemberData(nameof(Data))]
        public void VerifyExecution(IEnumerable<IBehaviourTreeNode<int, int>> nodes, BehaviourTreeState expectedState)
        {
            var selectNode = new SequenceNode<int, int>("test", false);

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
                    BehaviourTreeState.Failure
                };

                yield return new object[] {
                    new [] { fail.Object, fail.Object , fail.Object },
                    BehaviourTreeState.Failure
                };

                yield return new object[] {
                    new [] { fail.Object, running.Object, success.Object, fail.Object },
                    BehaviourTreeState.Failure
                };

                yield return new object[] {
                    new [] { success.Object, running.Object, success.Object, fail.Object },
                    BehaviourTreeState.Running
                };

                yield return new object[] {
                    new [] { success.Object, success.Object, success.Object },
                    BehaviourTreeState.Success
                };
            }
        }
    }
}
