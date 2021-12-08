namespace AdventOfCode.Solutions;

public class Day09 : AdventOfCodeBase
{
    
    public Day09()
    {
        Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
    }

    public override string AnswerPartOne()
    {
        var answer = 0;
 
        return $"Answer 1: {answer}";
    }
}