namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/1
/// </summary>
public class Day01 : AdventOfCodeBase
{
    private readonly Memory<int> _sonarDepths;

    public Day01()
    {
        Assert.True(File.Exists(this.InputFilename));
        _sonarDepths = File.ReadAllLines(this.InputFilename).Where(l => !string.IsNullOrEmpty(l)).Select(l => int.Parse(l)).ToArray();
    }

    public override string AnswerPartOne()
    {
        int? previousDept = null;
        int increaseCount = 0;
        foreach (var sonarDept in _sonarDepths.Span)
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
        for (var index = 0; index < _sonarDepths.Span.Length; index++)
        {
            int size = Math.Min(3, _sonarDepths.Span.Length - index);
            if (size != 3) continue;
            var window = _sonarDepths.Span.Slice(index, size);

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