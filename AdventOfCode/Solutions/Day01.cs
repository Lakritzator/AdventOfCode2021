using System;
using System.IO;
using System.Linq;
using Xunit;
using JM.LinqFaster;

namespace AdventOfCode.Solutions
{
    public class Day01 : AdventOfCodeBase
    {
        private readonly Memory<int> _sonarDepts;

        public Day01()
        {
            Assert.True(File.Exists(this.InputFilename));
            _sonarDepts = File.ReadAllLines(this.InputFilename).Where(l => !string.IsNullOrEmpty(l)).Select(l => int.Parse(l)).ToArray();
        }

        public override string AnswerPartOne()
        {
            int? previousDept = null;
            int increaseCount = 0;
            foreach (var sonarDept in _sonarDepts.Span)
            {
                if (sonarDept > previousDept)
                {
                    increaseCount++;
                }

                previousDept = sonarDept;
            }
            return $"Answer 1: {increaseCount}";
        }

        public override string AnswerPartTwo()
        {
            int? previousDeptSum = null;
            int increaseCount = 0;
            for (var index = 0; index < _sonarDepts.Span.Length; index++)
            {
                int size = Math.Min(3, _sonarDepts.Span.Length - index);
                if (size != 3) continue;
                var window = _sonarDepts.Span.Slice(index, size);

                var sum = window.SumF();
                if (sum > previousDeptSum)
                {
                    increaseCount++;
                }

                previousDeptSum = sum;
            }

            return $"Answer 2: {increaseCount}";
        }

    }
}
