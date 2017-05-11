using Moq;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Tests
{
    public class ParallelNodeTests
    {
        [Theory, MemberData(nameof(GetComilationTestData))]
        public void Compilation(IEnumerable<IBehaviourTreeNode<int, int>> nodes, bool success)
        {
            var parallelNode = new ParallelNode<int, int>("test", 1, 1);

            foreach (var node in nodes)
            {
                parallelNode.AddNode(node);
            }

            if (success)
            {
                var func = parallelNode.Compile();

                Assert.NotNull(func);
            }
            else
            {
                Assert.Throws<BehaviourTreeCompilationException>(() => parallelNode.Compile());
            }
        }

        public static IEnumerable<object[]> GetComilationTestData()
        {
            var obj = new Mock<IBehaviourTreeNode<int, int>>();

            yield return new object[] { new IBehaviourTreeNode<int, int>[0], false };
            yield return new object[] { new[] { obj.Object }, false };
            yield return new object[] { new[] { obj.Object, obj.Object }, true };
        }

        [Theory, MemberData(nameof(GetExecutionTestData))]
        public void Execution(IEnumerable<IBehaviourTreeNode<int, int>> nodes, int successCount, int failureCount, BehaviourTreeState expectedState)
        {
            var parallelNode = new ParallelNode<int, int>("test", successCount, failureCount);

            foreach (var node in nodes)
            {
                parallelNode.AddNode(node);
            }

            var func = parallelNode.Compile();

            Assert.Equal(expectedState, func(1, 1));
        }

        public static IEnumerable<object[]> GetExecutionTestData()
        {
            var successNode = new Mock<IBehaviourTreeNode<int, int>>();
            var failureNode = new Mock<IBehaviourTreeNode<int, int>>();
            var runningNode = new Mock<IBehaviourTreeNode<int, int>>();
            var errorNode = new Mock<IBehaviourTreeNode<int, int>>();

            successNode.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Success);
            failureNode.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Failure);
            runningNode.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Running);
            errorNode.Setup(f => f.Compile()).Returns((tick, state) => BehaviourTreeState.Error);


            yield return new object[] { new[] { successNode.Object, successNode.Object, failureNode.Object, errorNode.Object }, 3, 3, BehaviourTreeState.Error };
            yield return new object[] { new[] { successNode.Object, successNode.Object, failureNode.Object, errorNode.Object }, 3, 1, BehaviourTreeState.Failure };
            yield return new object[] { new[] { successNode.Object, successNode.Object, failureNode.Object, errorNode.Object }, 2, 1, BehaviourTreeState.Success };
            yield return new object[] { new[] { failureNode.Object, failureNode.Object, successNode.Object }, 1, 3, BehaviourTreeState.Success };
            yield return new object[] { new[] { failureNode.Object, successNode.Object }, 2, 2, BehaviourTreeState.Running };
        }
    }
}
