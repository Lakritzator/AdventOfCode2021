namespace AdventOfCode.Solutions;

public class Day02 : AdventOfCodeBase
{
    private readonly List<string> _submarineCommands;

    public Day02()
    {
        Assert.True(File.Exists(this.InputFilename));
        _submarineCommands = File.ReadAllLines(this.InputFilename).Where(l => !string.IsNullOrEmpty(l)).ToList();
    }

    public override string AnswerPartOne()
    {
        var position = 0;
        var depth = 0;

        foreach (var submarineCommand in _submarineCommands)
        {
            var commandInformation = submarineCommand.Split(" ");
            var command = commandInformation[0];
            var x = int.Parse(commandInformation[1]);
            switch (command)
            {
                case "forward":
                    position += x;
                    break;
                case "down":
                    depth += x;
                    break;
                case "up":
                    depth -= x;
                    break;
            }
        }
        return $"Answer 1: {position*depth}";
    }

    public override string AnswerPartTwo()
    {
        var position = 0;
        var depth = 0;
        var aim = 0;
        foreach (var submarineCommand in _submarineCommands)
        {
            var commandInformation = submarineCommand.Split(" ");
            var command = commandInformation[0];
            var x = int.Parse(commandInformation[1]);
            switch (command)
            {
                case "forward":
                    position += x;
                    depth += aim * x;
                    break;
                case "down":
                    aim += x;
                    break;
                case "up":
                    aim -= x;
                    break;
            }
        }

        return $"Answer 2: {position * depth}";
    }

}