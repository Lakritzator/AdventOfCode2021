namespace AdventOfCode.Solutions;

public class Day09 : AdventOfCodeBase
{
    private Map2D<int> _depthMap2D;

    public Day09()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));

        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        _depthMap2D = new Map2D<int>(lines[0].Length, lines.Length);

        for (int y = 0; y < _depthMap2D.Height; y++)
        {
            var depths = lines[y].AsSpan();
            for (int x = 0; x < _depthMap2D.Width; x++)
            {
                _depthMap2D[x, y] = depths[x]-'0';
            }
        }
    }

    public override string AnswerPartOne()
    {
        var answer = 0;

        foreach (var deepestPoint in DeepestPoints(_depthMap2D))
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
        var doneMap = new Map2D<bool>(_depthMap2D.Width, _depthMap2D.Height);

        var areas = new List<List<Point>>();

        // Use the deepest points as the starting point for the basin calculations
        foreach (var deepestPoint in DeepestPoints(_depthMap2D))
        {
            if (doneMap[deepestPoint.point])
            {
                continue;
            }
            var queueIn = new Queue<Point>();
            queueIn.Enqueue(deepestPoint.point);
            var queueOut = new Queue<Point>();
            var basinSize = 0;
            while (queueIn.Count > 0)
            {
                while (queueIn.TryDequeue(out var currentPoint))
                {
                    // Did we already map this point?
                    if (doneMap[currentPoint])
                    {
                        continue;
                    }

                    doneMap[currentPoint] = true;
                    basinSize++;
                    var under = currentPoint.Under();
                    if (_depthMap2D.IsInMap(under) && !doneMap[under] && _depthMap2D[under] < 9) {
                        queueOut.Enqueue(under);
                    }
                    var above = currentPoint.Above();
                    if (_depthMap2D.IsInMap(above) && !doneMap[above] && _depthMap2D[above] < 9)
                    {
                        queueOut.Enqueue(above);
                    }
                    var left = currentPoint.Left();
                    if (_depthMap2D.IsInMap(left) && !doneMap[left] && _depthMap2D[left] < 9)
                    {
                        queueOut.Enqueue(left);
                    }
                    var right = currentPoint.Right();
                    if (_depthMap2D.IsInMap(right) && !doneMap[right] && _depthMap2D[right] < 9)
                    {
                        queueOut.Enqueue(right);
                    }
                }
                queueIn = queueOut;
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

    private IEnumerable<(Point point, int value)> DeepestPoints(Map2D<int> depthMap)
    {
        for (int x = 0; x < depthMap.Width; x++)
        {
            for (int y = 0; y < depthMap.Height; y++)
            {
                if (IsDeepestPoint(depthMap, x, y))
                {
                    yield return (new Point(x,y), depthMap[x, y]);
                }
            }
        }
    }

    private bool IsDeepestPoint(Map2D<int> depthMap, int x, int y)
    {
        if (!depthMap.IsInMap(x, y))
        {
            return false;
        }
        var depth = depthMap[x, y];
        if (depthMap.IsInMap(x - 1, y) && depthMap[x - 1, y] <= depth) {
            return false;
        }
        if (depthMap.IsInMap(x + 1, y) && depthMap[x + 1, y] <= depth)
        {
            return false;
        }
        if (depthMap.IsInMap(x, y - 1) && depthMap[x, y - 1] <= depth)
        {
            return false;
        }
        if (depthMap.IsInMap(x, y + 1) && depthMap[x, y + 1] <= depth)
        {
            return false;
        }
        return true;
    }
}