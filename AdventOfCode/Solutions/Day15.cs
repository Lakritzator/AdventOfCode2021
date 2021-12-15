using System.Diagnostics;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/15
/// </summary>
public class Day15 : AdventOfCodeBase
{
    private Grid<int> _chitons;

    public Day15()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        _chitons = new Grid<int>(lines[0].Length, lines.Length);

        for (int y = 0; y < _chitons.Height; y++)
        {
            var risks = lines[y].AsSpan();
            for (int x = 0; x < _chitons.Width; x++)
            {
                _chitons[x, y] = risks[x] - '0';
            }
        }
    }

    public override string AnswerPartOne()
    {
        Debug.WriteLine("Chitons map part 1: \r\n" + _chitons);
        var start = new Point(0, 0);
        var end = new Point(_chitons.Width - 1, _chitons.Height - 1);
        var answer = CalculateLowestRiskPath(start, end, _chitons);
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        // Stencil the initial grid to a 5x grid, with small alterations
        var fiveTimeChitons = new Grid<int>(_chitons.Width * 5, _chitons.Height * 5);
        for (int y = 0; y < 5; y++)
        {
            var stencil = _chitons.Clone();
            foreach (var point in stencil)
            {
                var newValue = stencil[point] + y;
                if (newValue > 9)
                {
                    newValue -= 9;
                }
                stencil[point] = newValue;
            }
            for (int x = 0; x < 5; x++)
            {
                var destination = new Point(_chitons.Width * x, _chitons.Height * y);
                // Draw the source, but modified, to the specified location
                stencil.DrawTo(fiveTimeChitons, destination);
                foreach (var point in stencil)
                {
                    var newValue = stencil[point] + 1;
                    if (newValue > 9)
                    {
                        newValue = 1;
                    }
                    stencil[point] = newValue;
                }
            }
        }
        Debug.WriteLine("Chitons map part 2: \r\n" + fiveTimeChitons);

        var start = new Point(0, 0);
        var end = new Point(fiveTimeChitons.Width - 1, fiveTimeChitons.Height - 1);

        var answer = CalculateLowestRiskPath(start, end, fiveTimeChitons);
        return $"Answer 2: {answer}";
    }

    /// <summary>
    /// Use Dijkstra to calculate the path with the lowest risk
    /// </summary>
    /// <param name="start">Point</param>
    /// <param name="target">Point</param>
    /// <param name="riskGrid">Grid with the risks</param>
    /// <returns>lowest risk</returns>
    private static int CalculateLowestRiskPath(Point start, Point target, Grid<int> riskGrid)
    {
        var visitedGrid = riskGrid.CreateEmptyLike<bool>();
        var distanceGrid = riskGrid.CreateEmptyLike<int>().Clear(int.MaxValue);

        var next = new PriorityQueue<Point, int>();
        distanceGrid[start] = 0;
        next.Enqueue(start, 0);
        while (next.Count > 0)
        {
            var current = next.Dequeue();
            if (visitedGrid[current])
            {
                continue;
            }

            visitedGrid[current] = true;

            if (current == target)
            {
                return distanceGrid[target];
            }

            // Find all possible neighbors
            var neighbours = current.PointsAround().Where(riskGrid.IsValid);
            foreach (var neighbour in neighbours)
            {
                var distance = distanceGrid[current] + riskGrid[neighbour];

                if (distance < distanceGrid[neighbour])
                {
                    distanceGrid[neighbour] = distance;
                }

                if (distanceGrid[neighbour] != int.MaxValue)
                {
                    next.Enqueue(neighbour, distanceGrid[neighbour]);
                }
            }
        }
        return distanceGrid[target];
    }
}