using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/14
/// </summary>
public class Day14 : AdventOfCodeBase
{
    private string _polymerTemplate;
    private readonly IDictionary<string, string> _pairInsertionRules = new Dictionary<string, string>();

    public Day14()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));

        foreach (var line in File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)))
        {
            if (_polymerTemplate == null)
            {
                _polymerTemplate = line;
                continue;
            }

            var insertionData = line.SplitClean("->");
            _pairInsertionRules[insertionData[0]] = insertionData[1];
        }
    }

    public override string AnswerPartOne()
    {
        StringBuilder polymerBuilding = new();
        StringBuilder template = new(_polymerTemplate);

        int step = 0;
        while (step < 10)
        {
            polymerBuilding.Clear();
            polymerBuilding.Append(template[0]);
            for (var index = 0; index < template.Length - 1; index++)
            {
                var pair = $"{template[index]}{template[index + 1]}";
                polymerBuilding.Append(_pairInsertionRules[pair]);
                polymerBuilding.Append(template[index+1]);
            }

            (template, polymerBuilding) = (polymerBuilding, template);
            step++;
        }
        // Count and finish up
        var elementCount = new Dictionary<char, long>();
        for (var index = 0; index < template.Length; index++)
        {
            char element = template[index];
            if (!elementCount.TryGetValue(element, out long count))
            {
                elementCount[element] = 0;
            }
            elementCount[element] += 1;
        }

        var answer = elementCount.Keys.Select(key => elementCount[key]).Max() - elementCount.Keys.Select(key => elementCount[key]).Min();
        return $"Answer 1: {answer}";
    }
    
    private void ProcessPair(string pair, int step, int steps, Dictionary<char, long> elementCount)
    {

    }
}