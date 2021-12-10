using System.Diagnostics;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/11
/// </summary>
public class Day11 : AdventOfCodeBase
{

    public Day11()
    {
        //Initialize(this.InputFilename);
        Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        // = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
    }

    public override string AnswerPartOne()
    {
        int answer = 0;
        return $"Answer 1: {answer}";
    }
}