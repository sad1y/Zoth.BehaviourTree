
using Moq;
using System.Collections.Generic;
using Xunit;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Tests
{
    public class RandomSelectNodeTests
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
                return BehaviourTreeState.Success;
            });

            action2.Setup(f => f.Compile()).Returns((t, s) =>
            {
                callCount2++;
                return BehaviourTreeState.Success;
            });

            action3.Setup(f => f.Compile()).Returns((t, s) =>
            {
                callCount3++;
                return BehaviourTreeState.Success;
            });

            var randomSequence = new RandomSelectNode<int, int>("test");

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

        [Theory, MemberData(nameof(GetStatelessData))]
        public void VerifyExecution(IEnumerable<IBehaviourTreeNode<int, int>> nodes, BehaviourTreeState expectedState)
        {
            var selectNode = new RandomSelectNode<int, int>("test");

            foreach (var node in nodes)
            {
                selectNode.AddNode(node, 1);
            }

            var func = selectNode.Compile();

            var state = func(0, 0);

            Assert.Equal(expectedState, state);
        }

        [Fact]
        public void VerifyStatefulExecution()
        {
            var node1CallCount = 0;
            var node2CallCount = 0;
            var node3CallCount = 0;

            var node1 = new Mock<IBehaviourTreeNode<int, int>>();
            
            node1.Setup(f => f.Compile()).Returns((tick, state) => {
                node1CallCount++;
                return BehaviourTreeState.Failure;
            });

            var node2 = new Mock<IBehaviourTreeNode<int, int>>();

            node2.Setup(f => f.Compile()).Returns((tick, state) => {
                node2CallCount++;
                return BehaviourTreeState.Failure;
            });

            var node3 = new Mock<IBehaviourTreeNode<int, int>>();

            node3.Setup(f => f.Compile()).Returns((tick, state) => {
                node3CallCount++;
                return node3CallCount > 2 ? BehaviourTreeState.Failure : BehaviourTreeState.Running;
            });

            var selectNode = new RandomSelectNode<int, int>("test", stateful: true);

            selectNode.AddNode(node1.Object, 33);
            selectNode.AddNode(node2.Object, 33);
            selectNode.AddNode(node3.Object, 33);

            var func = selectNode.Compile();

            while(true)
            {
                if (func(1, 1) != BehaviourTreeState.Running) break;
            }

            Assert.Equal(1, node1CallCount);
            Assert.Equal(1, node2CallCount);
            Assert.Equal(3, node3CallCount);
        }

        public static IEnumerable<object[]> GetStatelessData()
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
                    new [] { error.Object },
                    BehaviourTreeState.Error
                };

            yield return new object[] {
                    new [] { fail.Object, fail.Object, success.Object },
                    BehaviourTreeState.Success
                };

            yield return new object[] {
                    new [] { fail.Object, fail.Object , fail.Object },
                    BehaviourTreeState.Failure
                };

            yield return new object[] {
                    new [] { fail.Object, running.Object, fail.Object },
                    BehaviourTreeState.Running
                };
        }
    }
}
