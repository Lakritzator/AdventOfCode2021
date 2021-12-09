namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/5
/// </summary>
public class Day05 : AdventOfCodeBase
{
    private IReadOnlyList<Line> _lines;
    private Map2D<int> _diagram;

    public override string AnswerPartOne()
    {
        Initialize(this.InputFilename);
        foreach (var line in _lines)
        {
            if (line.P1.X == line.P2.X || line.P1.Y == line.P2.Y)
            {
                _diagram.DrawLine(line, i => i+1);
            }
        }

        int answer = CountOverlaps(2);

        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        Initialize(this.InputFilename);
        foreach (var line in _lines)
        {
            _diagram.DrawLine(line, i => i+1);
        }
        int answer = CountOverlaps(2);

        return $"Answer 2: {answer}";
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));

        int maxX = 0, maxY = 0;

        var lines = new List<Line>();
        foreach (var lineData in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(lineData))
            {
                continue;
            }
            var pointData = lineData.SplitClean("->");
            var p1Data = pointData[0].SplitClean(',');
            var p1 = new Point(int.Parse(p1Data[0]), int.Parse(p1Data[1]));
            maxX = Math.Max(maxX, p1.X);
            maxY = Math.Max(maxY, p1.Y);
            var p2Data = pointData[1].SplitClean(',');
            var p2 = new Point(int.Parse(p2Data[0]), int.Parse(p2Data[1]));
            maxX = Math.Max(maxX, p2.X);
            maxY = Math.Max(maxY, p2.Y);
            lines.Add(new Line(p1, p2));
        }

        _diagram = new Map2D<int>(maxX + 1, maxY + 1);
        _lines = lines;
    }

    private int CountOverlaps(int min)
    {
        int count = 0;
        for (int x = 0; x < _diagram.Width; x++)
        {
            for (int y = 0; y < _diagram.Height; y++)
            {
                if (_diagram[x, y] >= min)
                {
                    count++;
                }
            }
        }

        return count;
    }
}