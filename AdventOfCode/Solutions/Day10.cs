using System.Diagnostics;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/10
/// </summary>
public class Day10 : AdventOfCodeBase
{
    private IReadOnlyList<string> _subSystem;

    private readonly IDictionary<char, int> _closingErrorScores = new Dictionary<char, int>
    {
        {')',3},
        {']',57},
        {'}',1197},
        {'>',25137},
    };

    private readonly IDictionary<char, int> _closingCompletionScores = new Dictionary<char, int>
    {
        {')',1},
        {']',2},
        {'}',3},
        {'>',4},
    };

    private readonly IDictionary<char, char> _matchingOpenCloseChars = new Dictionary<char, char>
    {
        {'(',')'},
        {'[',']'},
        {'{','}'},
        {'<','>'},
    };

    public Day10()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        _subSystem = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
    }

    public override string AnswerPartOne()
    {
        int answer = 0;
        foreach (var subSystemLine in _subSystem)
        {
            var parseStack = new Stack<char>();
            char? parseErrorOn = null;
            foreach (var foundChar in subSystemLine.AsSpan())
            {
                if (_matchingOpenCloseChars.ContainsKey(foundChar))
                {
                    parseStack.Push(foundChar);
                }
                else if (_closingErrorScores.ContainsKey(foundChar))
                {
                    var previousOpeningChar = parseStack.Pop();
                    var expectedChar = _matchingOpenCloseChars[previousOpeningChar];
                    if (expectedChar == foundChar)
                    {
                        continue;
                    }
                    Debug.WriteLine($"Expected {expectedChar} , but found {foundChar} instead.");
                    parseErrorOn = foundChar;
                    break;
                }

            }
            if (parseErrorOn.HasValue)
            {
                answer += _closingErrorScores[parseErrorOn.Value];
            }
        }
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        List<long> totalScores = new();

        foreach (var subSystemLine in _subSystem)
        {
            var parseStack = new Stack<char>();
            char? parseErrorOn = null;
            foreach (var foundChar in subSystemLine.AsSpan())
            {
                if (_matchingOpenCloseChars.ContainsKey(foundChar))
                {
                    parseStack.Push(foundChar);
                }
                else if (_closingErrorScores.ContainsKey(foundChar))
                {
                    var previousOpeningChar = parseStack.Pop();
                    var expectedChar = _matchingOpenCloseChars[previousOpeningChar];
                    if (expectedChar == foundChar)
                    {
                        continue;
                    }
                    Debug.WriteLine($"Expected {expectedChar} , but found {foundChar} instead.");
                    parseErrorOn = foundChar;
                    break;
                }
            }

            // Skip error lines
            if (parseErrorOn.HasValue)
            {
                continue;
            }

            // Calculate score
            long score = 0;
            while (parseStack.TryPop(out var openingChar))
            {
                score *= 5;
                score += _closingCompletionScores[_matchingOpenCloseChars[openingChar]];
            }
            totalScores.Add(score);
        }
        // find middle score
        var middle = totalScores.Count / 2;
        var sortedScores = totalScores.OrderBy(s => s).ToList();
        long answer = sortedScores[middle];

        return $"Answer 2: {answer}";
    }
}