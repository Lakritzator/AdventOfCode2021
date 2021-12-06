namespace AdventOfCode.Solutions;

public class Day07 : AdventOfCodeBase
{
    public Day07()
    {
        Initialize(this.InputFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        var input = File.ReadAllLines(path).First().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse);
    }

    public override string AnswerPartOne()
    {
        long answer = 0;

        return $"Answer 1: {answer}";
    }
}