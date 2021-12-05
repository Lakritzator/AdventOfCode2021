namespace AdventOfCode.Solutions;

public record Point(int X, int Y);

public record Line(Point P1, Point P2);

public class Day05 : AdventOfCodeBase
{
    private IReadOnlyList<Line> _lines;
    private int[,] _diagram;
    private int _width, _height;

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
            var pointData = lineData.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var p1Data = pointData[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var p1 = new Point(int.Parse(p1Data[0]), int.Parse(p1Data[1]));
            maxX = Math.Max(maxX, p1.X);
            maxY = Math.Max(maxY, p1.Y);
            var p2Data = pointData[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var p2 = new Point(int.Parse(p2Data[0]), int.Parse(p2Data[1]));
            maxX = Math.Max(maxX, p2.X);
            maxY = Math.Max(maxY, p2.Y);
            lines.Add(new Line(p1, p2));
        }

        _width = maxX+1;
        _height = maxY+1;
        _lines = lines;
        _diagram = new int[_width, _height];
    }

    private void DrawLine(Line line)
    {
        int x = line.P1.X;
        int y = line.P1.Y;
        int w = line.P2.X - line.P1.X;
        int h = line.P2.Y - line.P1.Y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        dx1 = w switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => dx1
        };
        dy1 = h switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => dy1
        };
        dx2 = w switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => dx2
        };
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            dy2 = h switch
            {
                < 0 => -1,
                > 0 => 1,
                _ => dy2
            };
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            _diagram[x, y]++;
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }

    private int CountOverlaps(int min)
    {
        int count = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_diagram[x, y] >= min)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public override string AnswerPartOne()
    {
        Initialize(this.InputFilename);
        foreach (var line in _lines)
        {
            if (line.P1.X == line.P2.X || line.P1.Y == line.P2.Y)
            {
                DrawLine(line);
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
            DrawLine(line);
        }
        int answer = CountOverlaps(2);

        return $"Answer 2: {answer}";
    }
}