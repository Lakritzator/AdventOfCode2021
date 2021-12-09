namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/7
/// </summary>
public class Day07 : AdventOfCodeBase
{
    private int[] _input;
    public Day07()
    {
        Initialize(this.InputFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        _input = File.ReadAllLines(path).First().SplitClean(',').Select(int.Parse).OrderBy(pos => pos).ToArray();
    }

    public override string AnswerPartOne()
    {
        var maxPos = _input.Max();
        var optimalPos = int.MaxValue;
        for (int horizontalPos = 0; horizontalPos <= maxPos; horizontalPos++)
        {
            var possibleOptimalPos = _input.Select(h => CalculateFuelPartOne(h, horizontalPos)).Sum();
            if (optimalPos > possibleOptimalPos)
            {
                optimalPos = possibleOptimalPos;
            }
        }
        return $"Answer 1: {optimalPos}";
    }

    public override string AnswerPartTwo()
    {
        var maxPos = _input.Max();
        var optimalPos = int.MaxValue;
        for (int horizontalPos = 0; horizontalPos <= maxPos; horizontalPos++)
        {
            var possibleOptimalPos = _input.Select(h => CalculateFuelPartTwo(h, horizontalPos)).Sum();
            if (optimalPos > possibleOptimalPos)
            {
                optimalPos = possibleOptimalPos;
            }
        }
        return $"Answer 1: {optimalPos}";
    }

    private static int CalculateFuelPartOne(int crabPosition, int targetPosition)
    {
        var distance = Math.Abs(targetPosition - crabPosition);

        return distance;
    }

    private static int CalculateFuelPartTwo(int crabPosition, int targetPosition)
    {
        var distance = Math.Abs(targetPosition - crabPosition);

        return distance * (distance + 1) / 2;
    }
}