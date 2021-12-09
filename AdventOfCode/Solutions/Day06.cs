namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/6
/// </summary>
public class Day06 : AdventOfCodeBase
{
    private readonly long[] _initialValues = new long[9];

    public Day06()
    {
        Initialize(this.InputFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        var input = File.ReadAllLines(path).First().SplitClean(',').Select(int.Parse);

        foreach (var i in input)
        {
            _initialValues[i]++;
        }
    }

    public override string AnswerPartOne()
    {
        var currentDay = new long[9];
        var newDay = new long[9];

        for (int i = 0; i < _initialValues.Length; i++)
        {
            currentDay[i] = _initialValues[i];
        }
        int day = 0;
        while (day < 80)
        {
            newDay[0] = currentDay[1];
            newDay[1] = currentDay[2];
            newDay[2] = currentDay[3];
            newDay[3] = currentDay[4];
            newDay[4] = currentDay[5];
            newDay[5] = currentDay[6];
            newDay[6] = currentDay[7];
            newDay[7] = currentDay[8];
            newDay[8] = currentDay[0];
            newDay[6] += currentDay[0];

            // Swap
            (newDay, currentDay) = (currentDay, newDay);
            day++;
        }

        long answer = currentDay.SumF();

        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        var currentDay = new long[9];
        var newDay = new long[9];

        for (int i = 0; i < _initialValues.Length; i++)
        {
            currentDay[i] = _initialValues[i];
        }
        int day = 0;
        while (day < 256)
        {
            newDay[0] = currentDay[1];
            newDay[1] = currentDay[2];
            newDay[2] = currentDay[3];
            newDay[3] = currentDay[4];
            newDay[4] = currentDay[5];
            newDay[5] = currentDay[6];
            newDay[6] = currentDay[7];
            newDay[7] = currentDay[8];
            newDay[8] = currentDay[0];
            newDay[6] += currentDay[0];

            // Swap
            (newDay, currentDay) = (currentDay, newDay);
            day++;
        }

        long answer = currentDay.SumF();

        return $"Answer 2: {answer}";
    }
}