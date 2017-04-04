using Moq;
using Xunit;
using Zoth.BehaviourTree.Builders;
using Zoth.BehaviourTree.Exceptions;
using Zoth.BehaviourTree.Extentions;

namespace Zoth.BehaviourTree.Tests
{
    public class FluentBehaviourTreeBuilderTests
    {
        [Fact]
        public void AddNodeThatIsNotACompositionNode()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            var moqAction = new Mock<IBehaviourTreeNode<int, int>>();

            Assert.Throws<BehaviourTreeBuilderException>(() => builder.Add(moqAction.Object));
        }

        [Fact]
        public void AddNodeTwice()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            var moqAction = new Mock<IBehaviourTreeNodeSequence<int, int>>();

            Assert.Throws<BehaviourTreeBuilderException>(() =>
            {
                builder.Add(moqAction.Object);
                builder.Add(moqAction.Object);
            });
        }

        [InlineData(1)]
        [InlineData(10)]
        [Theory]
        public void InvokeEndAnyNumberOfTimes(int count)
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            for (int i = 0; i < count; i++)
            {
                builder.End();
            }
        }

        [Fact]
        public void BuildWihtoutAnySetup()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            Assert.Throws<BehaviourTreeBuilderException>(() => {
                builder.Build();
            });
        }

        [Fact]
        public void Build()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            var moqAction = new Mock<IBehaviourTreeNodeSequence<int, int>>();

            moqAction.Setup(f => f.Compile()).Returns((time, state) => BehaviourTreeState.Running);

            builder.Add(moqAction.Object);

            var action = builder.Build();

            Assert.NotNull(action);
            Assert.Equal(BehaviourTreeState.Running, action(1, 1));
        }

        [Fact]
        public void SetupNestedTree()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();
            var moqAction = new Mock<IBehaviourTreeNodeSequence<int, int>>();
            var moqProfile = new Mock<IActionProfiler<int>>();

            moqAction.Setup(f => f.Compile()).Returns((time, state) => BehaviourTreeState.Running);

            builder
                .Root(moqAction.Object, b => {
                    b.Select("", selectB => {
                        selectB.Sequence("", )
                    })
                });

            var subtreeBuilder = builder
                .Add(moqAction.Object)
                    .Add(moqAction.Object)
                        .Add(moqAction.Object)
                        .End()
                    .Add(moqAction.Object)
                    .End();

            Assert.NotEqual(builder, subtreeBuilder);
            Assert.Equal(subtreeBuilder, subtreeBuilder.End());
        }
    }
}
