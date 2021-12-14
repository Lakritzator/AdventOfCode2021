namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/14
/// </summary>
public class Day14 : AdventOfCodeBase
{
    private string _polymerTemplate;
    private readonly Dictionary<string, char> _pairInsertionRules = new();
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
            _pairInsertionRules[insertionData[0]] = insertionData[1][0];
        }
    }

    public override string AnswerPartOne()
    {
        var pairCount = InitializeCount(_polymerTemplate);

        // Count pairs
        var tmpPairCount = new Dictionary<string, long>();
        for (var step = 0; step < 10; step++)
        {
            CreateNewPairs(pairCount, tmpPairCount,_pairInsertionRules);
            (pairCount, tmpPairCount) = (tmpPairCount, pairCount);
        }

        // Count single chars
        var countArray = CountElements(pairCount, _polymerTemplate);

        var leastCommonElement = countArray.Where(c => c > 0).Min();
        var mostCommonElement = countArray.Max();

        var answer = (mostCommonElement - leastCommonElement) / 2;
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        var pairCount = InitializeCount(_polymerTemplate);

        // Count pairs
        var tmpPairCount = new Dictionary<string, long>();
        for (var step = 0; step < 40; step++)
        {
            CreateNewPairs(pairCount, tmpPairCount, _pairInsertionRules);
            (pairCount, tmpPairCount) = (tmpPairCount, pairCount);
        }

        // Count single chars
        var countArray = CountElements(pairCount, _polymerTemplate);

        var leastCommonElement = countArray.Where(c => c > 0).Min();
        var mostCommonElement = countArray.Max();

        var answer = (mostCommonElement - leastCommonElement) / 2;
        return $"Answer 2: {answer}";
    }

    /// <summary>
    /// Initialize the counting of the pairs
    /// </summary>
    /// <param name="polymerTemplate">string</param>
    /// <returns>Dictionary with pairs and their count</returns>
    private static Dictionary<string, long> InitializeCount(string polymerTemplate)
    {
        Dictionary<string, long> pairCount = new();
        // Initialize pairs
        for (var index = 0; index < polymerTemplate.Length - 1; index++)
        {
            var pair = $"{polymerTemplate[index]}{polymerTemplate[index + 1]}";
            pairCount[pair] = pairCount.GetValueOrDefault(pair) + 1;
        }

        return pairCount;
    }

    /// <summary>
    /// Make new pairs for the already existing ones
    /// </summary>
    /// <param name="source">Dictionary with pairs and their count to read from</param>
    /// <param name="destination">Dictionary with pairs and their count to create new ones</param>
    /// <param name="pairInsertionRules">Dictionary with the rules</param>
    private static void CreateNewPairs(Dictionary<string, long> source, Dictionary<string, long> destination, Dictionary<string, char> pairInsertionRules)
    {
        destination.Clear();
        foreach (var pair in source.Keys.ToList())
        {
            var currentCount = source.GetValueOrDefault(pair);
            var insert = pairInsertionRules[pair];
            var newPair1 = $"{pair[0]}{insert}";
            destination[newPair1] = destination.GetValueOrDefault(newPair1) + currentCount;
            var newPair2 = $"{insert}{pair[1]}";
            destination[newPair2] = destination.GetValueOrDefault(newPair2) + currentCount;
        }
    }

    /// <summary>
    /// Calculate the single elements in the pairs
    /// </summary>
    /// <param name="pairCount">Dictionary with the pairs and their count</param>
    /// <param name="polymerTemplate">string with the template, used to correct that the first and last were missing</param>
    /// <returns>Array of long with the count per element (char)</returns>
    private static long[] CountElements(Dictionary<string, long> pairCount, string polymerTemplate)
    {
        // Count single chars
        var countArray = new long[26];
        // Count first and last char, as these are not processed
        countArray[polymerTemplate[0] - 'A']++;
        countArray[polymerTemplate.Last() - 'A']++;
        foreach (var pair in pairCount.Keys)
        {
            var index = pair[0] - 'A';
            countArray[index] += pairCount[pair];
            index = pair[1] - 'A';
            countArray[index] += pairCount[pair];
        }

        return countArray;
    }
}