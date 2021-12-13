namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/9
/// </summary>
public class Day09 : AdventOfCodeBase
{
    private Grid<int> _depthGrid;

    public Day09()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));

        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        _depthGrid = new Grid<int>(lines[0].Length, lines.Length);

        for (int y = 0; y < _depthGrid.Height; y++)
        {
            var depths = lines[y].AsSpan();
            for (int x = 0; x < _depthGrid.Width; x++)
            {
                _depthGrid[x, y] = depths[x]-'0';
            }
        }
    }

    public override string AnswerPartOne()
    {
        var answer = 0;

        foreach (var deepestPoint in DeepestPoints(_depthGrid))
        {
            var riskLevel = deepestPoint.value + 1;
            answer += riskLevel;
        }
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        var answer = 1;
        var basinSizes = new List<int>();
        var doneMap = new Grid<bool>(_depthGrid.Width, _depthGrid.Height);

        // Use the deepest points as the starting point for the basin calculations
        foreach (var deepestPoint in DeepestPoints(_depthGrid))
        {
            if (doneMap[deepestPoint.point])
            {
                continue;
            }
            var stackIn = new Stack<Point>();
            stackIn.Push(deepestPoint.point);
            var stackOut = new Stack<Point>();
            var basinSize = 0;
            while (stackIn.Count > 0)
            {
                while (stackIn.TryPop(out var currentPoint))
                {
                    // Did we already map this point?
                    if (doneMap[currentPoint])
                    {
                        continue;
                    }

                    doneMap[currentPoint] = true;
                    basinSize++;
                    foreach (var pointAround in currentPoint.PointsAround())
                    {
                        if (_depthGrid.IsValid(pointAround) && !doneMap[pointAround] && _depthGrid[pointAround] < 9)
                        {
                            stackOut.Push(pointAround);
                        }
                    }
                }
                stackIn = stackOut;
            }
            basinSizes.Add(basinSize);
        }

        // Take the top 3 basin sizes and multiply their values
        foreach (var basinSize in basinSizes.OrderByDescending(v => v).Take(3))
        {
            answer *= basinSize;
        }
        return $"Answer 2: {answer}";
    }

    private static IEnumerable<(Point point, int value)> DeepestPoints(Grid<int> depthMap) => depthMap.Where(p => IsDeepestPoint(depthMap, p)).Select(point => (point, depthMap[point]));
    
    private static bool IsDeepestPoint(Grid<int> depthMap, Point point)
    {
        if (!depthMap.IsValid(point))
        {
            return false;
        }
        var depth = depthMap[point];
        foreach (var pointAround in point.PointsAround())
        {
            if (depthMap.IsValid(pointAround) && depthMap[pointAround] <= depth)
            {
                return false;
            }
        }
        return true;
    }
}