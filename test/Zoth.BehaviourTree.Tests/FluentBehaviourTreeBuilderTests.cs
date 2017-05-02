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
        public void AddNodeTwice()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            var moqAction = new Mock<IBehaviourTreeNodeSequence<int, int>>();

            Assert.Throws<BehaviourTreeBuilderException>(() =>
            {
                builder.Select(root => { });
                builder.Sequence(root => { });
            });
        }

        [Fact]
        public void BuildWihtoutAnySetup()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            Assert.Throws<BehaviourTreeBuilderException>(() =>
            {
                builder.Build();
            });
        }

        [Fact]
        public void Build()
        {
            var builder = new FluentBehaviourTreeBuilder<int, int>();

            var moqAction = new Mock<IBehaviourTreeNodeSequence<int, int>>();

            moqAction.Setup(f => f.Compile()).Returns((time, state) => BehaviourTreeState.Running);

            builder.Root(moqAction.Object, root => { });

            var action = builder.Build();

            var compiled = action.Compile();

            Assert.NotNull(action);
            Assert.Equal(BehaviourTreeState.Running, compiled(1, 1));
        }
    }
}
