
namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/12
/// </summary>
public class Day12 : AdventOfCodeBase
{
    private IReadOnlyDictionary<string, Cave> _caves;
    public Day12()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        // = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        _caves = BuidCaves(lines);
    }

    public override string AnswerPartOne()
    {
        // Count pathways
        int answer = 0;

        var completedPaths = new List<CavePath>();
        var pathStack = new Stack<CavePath>();

        // Build initial path
        var path = new CavePath(_caves["start"]);
        pathStack.Push(path);
        while (pathStack.TryPop(out var currentPath))
        {
            foreach (var destinationCaveName in currentPath.VisitedCaves.Last().ConnectedTo)
            {
                var destinationCave = _caves[destinationCaveName];
                var newPath = currentPath.Clone().VisitCave(destinationCave);

                if (destinationCave.IsEnd())
                {
                    completedPaths.Add(newPath);
                    continue;
                }
                // Check if we are allowed to visit the destination (if it is a small cave, it can only be visited once)
                if (!currentPath.AllowVisit(destinationCave))
                {
                    continue;
                }

                pathStack.Push(newPath);
            }
        }

        answer += completedPaths.Count;
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        // Count pathways
        int answer = 0;

        var completedPaths = new List<CavePath>();
        var pathStack = new Stack<CavePath>();

        // Build initial path
        var path = new CavePath(_caves["start"]);
        pathStack.Push(path);
        while (pathStack.TryPop(out var currentPath))
        {
            foreach (var destinationCaveName in currentPath.VisitedCaves.Last().ConnectedTo)
            {
                var destinationCave = _caves[destinationCaveName];

                var newPath = currentPath.Clone().VisitCave(destinationCave);

                if (destinationCave.IsEnd())
                {
                    completedPaths.Add(newPath);
                    continue;
                }

                bool allowDoubleVisit = false;
                // Check if we are allowed to visit the destination (if it is a small cave, it can only be visited once)
                if (destinationCave.IsSmallCave())
                {
                    // Did we already visit this cave?
                    if (currentPath.VisitedCaves.Any(cave => destinationCave.Name.Equals(cave.Name)))
                    {
                        // Is this the cave which we allowed to visit twice?
                        if (currentPath.AllowedSmallCaveDoubleVisit == destinationCaveName)
                        {
                            // Did we already visit it?
                            if (currentPath.AllowedSmallCaveDoubleVisited)
                            {
                                continue;
                            }

                            // Make sure we mark it as visited
                            newPath.AllowedSmallCaveDoubleVisited = true;
                        }
                        else
                        {
                            continue;
                        }

                    }
                    else if (currentPath.AllowedSmallCaveDoubleVisit == null)
                    {
                        // Allow to visit the current small cave once or twice
                        allowDoubleVisit = true;
                    }
                }
                pathStack.Push(newPath);
                if (!allowDoubleVisit)
                {
                    continue;
                }
                newPath = newPath.Clone();
                newPath.AllowedSmallCaveDoubleVisit = destinationCaveName;
                newPath.AllowedSmallCaveDoubleVisited = false;
                pathStack.Push(newPath);
            }
        }

        var finalPaths = completedPaths.Select(cp => cp.ToString()).Distinct().OrderBy(s => s).ToList();
        answer += finalPaths.Count;
        return $"Answer 2: {answer}";
    }
    private static IReadOnlyDictionary<string, Cave> BuidCaves(IEnumerable<string> lines)
    {
        Dictionary<string, Cave> caves = new();
        foreach (var line in lines)
        {
            var caveConnection = line.SplitClean('-');
            var caveName = caveConnection[0];
            var connectedTo = caveConnection[1];
            CreateCave(caveName, connectedTo, caves);
            if ("end".Equals(connectedTo) || "start".Equals(caveName))
            {
                continue;
            }
            CreateCave(connectedTo, caveName, caves);
        }

        return caves;
    }

    private static void CreateCave(string caveName, string connectedTo, Dictionary<string, Cave> caves)
    {
        if (!caves.TryGetValue(caveName, out var cave))
        {
            cave = new Cave(caveName);
        }
        cave.ConnectedTo.Add(connectedTo);
        caves[caveName] = cave;
        if (!caves.ContainsKey(connectedTo))
        {
            caves[connectedTo] = new Cave(connectedTo);
        }

    }
}

internal class CavePath
{
    private List<Cave> _visitedCaves = new();

    private CavePath()
    {
    }
    
    public CavePath(Cave startCave)
    {
        _visitedCaves.Add(startCave);
    }

    public IReadOnlyList<Cave> VisitedCaves => _visitedCaves;

    public bool AllowVisit(Cave destinationCave)
    {
        if (destinationCave.IsEnd())
        {
            return true;
        }
        // Check if the destination is a small cave, which can only be visited once
        if (!destinationCave.IsSmallCave())
        {
            return true;
        }
        return !VisitedCaves.Any(cave => destinationCave.Name.Equals(cave.Name));
    }

    public CavePath VisitCave(Cave caveToVisit)
    {
        _visitedCaves = _visitedCaves.Append(caveToVisit).ToList();
        return this;
    }

    public string AllowedSmallCaveDoubleVisit { get; set; }
    public bool AllowedSmallCaveDoubleVisited { get; set; }

    public CavePath Clone()
    {
        var cavePath = new CavePath
        {
            _visitedCaves = this._visitedCaves,
            AllowedSmallCaveDoubleVisit = this.AllowedSmallCaveDoubleVisit,
            AllowedSmallCaveDoubleVisited = this.AllowedSmallCaveDoubleVisited
        };
        return cavePath;
    }

    public override string ToString() => string.Join(",", _visitedCaves.Select(c => c.Name));
}
internal class Cave
{
    public string Name { get; }

    public bool IsEnd() => "end".Equals(Name);

    public bool IsSmallCave() => Name.All(char.IsLower);

    public Cave(string name)
    {
        Name = name;
        ConnectedTo = new HashSet<string>();
    }

    public ISet<string> ConnectedTo { get; }
}
