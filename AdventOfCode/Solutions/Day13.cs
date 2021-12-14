using System.Data;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/13
/// </summary>
public class Day13 : AdventOfCodeBase
{
    private static readonly char DOT = '█';
    private static readonly char EMPTY = ' ';
    private Grid<char> _dots;
    private IList<string> _foldInstructions;

    public Day13()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));

        var dotCoordinates = File.ReadAllLines(path).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(Point.Parse).ToArray();
        var maxX = dotCoordinates.Max(p => p.X)+1;
        var maxY = dotCoordinates.Max(p => p.Y)+1;
        _dots = new Grid<char>(maxX, maxY);
        _dots.Clear(EMPTY);
        foreach (var dotCoordinate in dotCoordinates)
        {
            _dots[dotCoordinate] = DOT;
        }
        _foldInstructions = File.ReadAllLines(path).Skip(dotCoordinates.Length).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
    }

    public override string AnswerPartOne()
    {
        Grid<char> beforeFold = _dots;
        Grid<char> afterFold = beforeFold;
        foreach (var foldInstruction in _foldInstructions)
        {
            var splitData = foldInstruction.SplitClean('=');
            int foldPosition = int.Parse(splitData[1]);
            afterFold = splitData[0] switch
            {
                "fold along x" => FoldLeft(beforeFold, foldPosition),
                "fold along y" => FoldUp(beforeFold, foldPosition),
                _ => throw new SyntaxErrorException(splitData[0])
            };
            break;
        }
        return $"Answer 1: {afterFold.Count(p => afterFold[p] ==DOT)}";
    }


    public override string AnswerPartTwo()
    {
        Grid<char> beforeFold = _dots;
        Grid<char> afterFold = null;
        foreach (var foldInstruction in _foldInstructions)
        {
            var splitData = foldInstruction.SplitClean('=');
            int foldPosition = int.Parse(splitData[1]);
            switch (splitData[0])
            {
                case "fold along x":
                    afterFold = FoldLeft(beforeFold, foldPosition);
                    break;
                case "fold along y":
                    afterFold = FoldUp(beforeFold, foldPosition);
                    break;
            }

            beforeFold = afterFold;
        }
        return $"Answer 2: \r\n{afterFold}";
    }

    private static Grid<char> FoldLeft(Grid<char> beforeFoldLeft, int foldX)
    {
        var afterFoldLeft = new Grid<char>(foldX, beforeFoldLeft.Height).Clear(EMPTY);

        foreach (var point in beforeFoldLeft)
        {
            if (beforeFoldLeft[point] != DOT)
            {
                continue;
            }
            if (point.X < foldX)
            {
                afterFoldLeft[point] = DOT;
                continue;
            }


            var leftFoldedDot = point with
            {
                X = foldX - (point.X - foldX)
            };

            afterFoldLeft[leftFoldedDot] = DOT;
        }

        return afterFoldLeft;
    }


    private static Grid<char> FoldUp(Grid<char> beforeFoldUp, int foldY)
    {
        var afterFoldUp = new Grid<char>(beforeFoldUp.Width, foldY).Clear(EMPTY);
        foreach (var point in beforeFoldUp)
        {
            if (beforeFoldUp[point] != DOT)
            {
                continue;
            }
            if (point.Y < foldY)
            {
                afterFoldUp[point] = DOT;
                continue;
            }

            var upFoldedDot = point with
            {
                Y = foldY - (point.Y - foldY)
            };

            afterFoldUp[upFoldedDot] = DOT;
        }

        return afterFoldUp;
    }
}