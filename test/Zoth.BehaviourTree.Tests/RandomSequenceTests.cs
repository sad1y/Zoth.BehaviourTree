
using Moq;
using System.Collections.Generic;
using Xunit;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Tests
{
    public class RandomSequenceTests
    {
        [Fact]
        public void CountRandomCall()
        {
            var action1 = new Mock<IBehaviourTreeNode<int, int>>();
            var action2 = new Mock<IBehaviourTreeNode<int, int>>();
            var action3 = new Mock<IBehaviourTreeNode<int, int>>();

            int callCount1 = 0, callCount2 = 0, callCount3 = 0;

            action1.Setup(f => f.Compile()).Returns((t, s) =>
            {
                callCount1++;
                return BehaviourTreeState.Failure;
            });

            action2.Setup(f => f.Compile()).Returns((t, s) =>
            {
                callCount2++;
                return BehaviourTreeState.Failure;
            });

            action3.Setup(f => f.Compile()).Returns((t, s) =>
            {
                callCount3++;
                return BehaviourTreeState.Failure;
            });

            var randomSequence = new RandomSequenceNode<int, int>("test");

            randomSequence.AddNode(action1.Object, 100);
            randomSequence.AddNode(action2.Object, 50);
            randomSequence.AddNode(action3.Object, 40);

            var func = randomSequence.Compile();

            for (int i = 0; i < 100; i++)
            {
                func(0, 0);
            }

            var condition = (callCount1 > callCount2) && (callCount2 > callCount3);

            Assert.True(condition);
        }

        [Theory, MemberData(nameof(Data))]
        public void VerifyExecution(IEnumerable<IBehaviourTreeNode<int, int>> nodes, BehaviourTreeState expectedState)
        {
            var selectNode = new RandomSequenceNode<int, int>("test");

            foreach (var node in nodes)
            {
                selectNode.AddNode(node, 1);
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
                    new [] { error.Object, success.Object, success.Object },
                    BehaviourTreeState.Error
                };

                yield return new object[] {
                    new [] { success.Object, success.Object, success.Object },
                    BehaviourTreeState.Success
                };

                yield return new object[] {
                    new [] { fail.Object, fail.Object, fail.Object },
                    BehaviourTreeState.Failure
                };

                yield return new object[] {
                    new [] { success.Object, fail.Object, fail.Object },
                    BehaviourTreeState.Failure
                };

                yield return new object[] {
                    new [] { success.Object, running.Object, success.Object, success.Object },
                    BehaviourTreeState.Running
                };
            }
        }

    }
}
