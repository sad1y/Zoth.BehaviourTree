using Moq;
using System;
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
    }
}
