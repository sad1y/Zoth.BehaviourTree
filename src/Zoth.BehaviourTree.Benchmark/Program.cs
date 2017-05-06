using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Running;
using System;
using System.Security.Cryptography;
using Zoth.BehaviourTree.Compilation;
using Zoth.BehaviourTree.Nodes;

namespace Zoth.BehaviourTree.Benchmark
{
    [Config(typeof(Config))]
    public class SequentailCallCompilerVsSequentailCallCompilerNew
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
            }
        }

        public SequentailCallCompilerVsSequentailCallCompilerNew()
        {
            var actionNode1 = new ActionNode<int, int>("action 1", (a, b) =>
            {
                b = a + b;
                return BehaviourTreeState.Success;
            });

            var actionNode2 = new ActionNode<int, int>("action 2", (a, b) =>
            {
                b = a * b;
                return BehaviourTreeState.Success;
            });

            var actionNode3 = new ActionNode<int, int>("action 3", (a, b) =>
            {
                b = a * b * b;
                return BehaviourTreeState.Success;
            });

            _wrapCallCompiler = new WrapCallCompiler<int, int>(new[] { actionNode1, actionNode2, actionNode3 }, (state) => state == BehaviourTreeState.Error).Compile(true);
            _seqCallCompiler = new SequentialCallCompiler<int, int>(new[] { actionNode1, actionNode2, actionNode3 }, (state) => state == BehaviourTreeState.Error).Compile(true);
            _randomCompiler = new RandomCallCompiler<int, int>(new[] {
                new RandomEntry<IBehaviourTreeNode<int, int>>(33, actionNode1),
                new RandomEntry<IBehaviourTreeNode<int, int>>(33, actionNode2),
                new RandomEntry<IBehaviourTreeNode<int, int>>(33, actionNode3),
            }, (state) => state == BehaviourTreeState.Error).Compile(true);
        }

        Func<int, int, BehaviourTreeState> _wrapCallCompiler;
        Func<int, int, BehaviourTreeState> _seqCallCompiler;
        Func<int, int, BehaviourTreeState> _randomCompiler;

        [Benchmark]
        public BehaviourTreeState Current() => _wrapCallCompiler(3, 7);

        [Benchmark]
        public BehaviourTreeState New() => _seqCallCompiler(3, 7);

        [Benchmark]
        public BehaviourTreeState Random() => _randomCompiler(3, 7);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SequentailCallCompilerVsSequentailCallCompilerNew>();

            Console.ReadKey();
        }
    }
}
