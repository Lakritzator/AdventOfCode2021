using System.Diagnostics;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/15
/// </summary>
public class Day15 : AdventOfCodeBase
{
    private Grid<int> _chitons;
    private Point _end;
    private Grid<bool> _visitedGrid;
    private int _overallLowestRisk = int.MaxValue;
    private int _maxSteps;
    public Day15()
    {
        //Initialize(this.InputFilename);
        Initialize(this.InputExampleFilename);
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
        _end = new Point(_chitons.Width - 1, _chitons.Height - 1);
        _visitedGrid = _chitons.CreateEmptyLike<bool>();
        _overallLowestRisk = (_chitons.Width + _chitons.Height) * 9;
        _maxSteps = (_chitons.Width + _chitons.Height) * 4;
        Debug.WriteLine("Chitons map: \r\n"+_chitons);
    }

    public override string AnswerPartOne()
    {
        var start = new Point(0, 0);
        var answer = FindPath(start, 0, 0);


        return $"Answer 1: {answer}";
    }

    private int FindPath(Point currentLocation, int currentRiskLevel, int step)
    {
        if (_end.Equals(currentLocation))
        {
            _overallLowestRisk = Math.Min(currentRiskLevel, _overallLowestRisk);
            Debug.WriteLine($"Reached end with risk level {currentRiskLevel} and {step} steps, current record is {_overallLowestRisk}");
            return currentRiskLevel;
        }
        // Do not go on this path when we have more steps than a maximum
        if (step > _maxSteps)
        {
            //Debug.WriteLine($"To many steps ({step}) breaking off with risk level {currentRiskLevel} (max {_overallLowestRisk}).");
            return int.MaxValue;
        }
        // Do not go on this path if we go above the current lowest risk
        if (currentRiskLevel > _overallLowestRisk)
        {
            return int.MaxValue;
        }

        // Visit, so we do not go that way again
        _visitedGrid[currentLocation] = true;
        int lowestRisk = int.MaxValue;

        // Find all possible steps, prefer the low risks first and if needed go down
        var orderedDestinations = currentLocation
            // Get the points around the current location (left, right, up, down)
            .PointsAround()
            // Take the ones that are inside the grid and not yet visited
            .Where(p => _chitons.IsValid(p) && !_visitedGrid[p])
            // Take the lowest risk first
            .OrderByDescending(p => p.X * p.Y).ToList();
        foreach (var destination in orderedDestinations)
        {
            var destinationRiskLevel = _chitons[currentLocation];
            var visitedRiskLevel = FindPath(destination, currentRiskLevel + destinationRiskLevel, step + 1);
            lowestRisk = Math.Min(lowestRisk, visitedRiskLevel);
            // If we get back, we can make a fail fast if the current risk level is already above the _overallLowestRisk
            if (currentRiskLevel > _overallLowestRisk)
            {
                _visitedGrid[currentLocation] = false;
                return int.MaxValue;
            }
        }
        // Leave current location as we are recursive
        _visitedGrid[currentLocation] = false;
        return lowestRisk;
    }
}