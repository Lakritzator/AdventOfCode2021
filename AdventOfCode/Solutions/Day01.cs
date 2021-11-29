using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode.Solutions
{
    public class Day01 : AdventOfCodeBase
    {
        &&private readonly InputType;

        public Day01()
        {
            Assert.True(File.Exists(this.InputFilename));
            var doSomething = File.ReadAllLines(this.InputFilename).Select(l => int.Parse(l)).ToList();
        }

        public override string AnswerPartOne()
        {

            return $"Answer 1";
        }

        public override string AnswerPartTwo()
        {
            return $"Answer 2";
        }

    }
}
