namespace AdventOfCode.Solutions;

public class Day08 : AdventOfCodeBase
{
    private List<(string [] left, string[] right)> _input;

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    private readonly List<string> _digitSegments = new(){
        "abcefg", // 0 - length=6
        "cf", // 1 - length=2 (unique)
        "acdeg", // 2 - length=5
        "acdfg", // 3 - length=5
        "bcdf", // 4 - length=4 (unique)
        "abdfg", // 5 - length=5
        "abdefg", // 6 - length=6
        "acf", // 7 - length=3 (unique)
        "abcdefg", // 8 - length=7 (unique)
        "abcdfg" // 9 - length=6
    };

    public Day08()
    {
        Initialize(this.InputFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));

        _input = File.ReadAllLines(path)
            // Separate left & right part
            .Select(l => l.SplitClean('|'))
            // Pick right part
            .Select(p => (
                p[0].SplitClean(' ').Select(signal => signal.Order()).ToArray(),
                p[1].SplitClean(' ').Select(signal => signal.Order()).ToArray()))
            .ToList();
    }

    public override string AnswerPartOne()
    {
        var answer = 0;
        // Count the easy known mappings in the input
        foreach (var content in _input)
        {
            foreach (var segmentInformation in content.right)
            {
                switch (segmentInformation.Length)
                {
                    // Digit 1 has 2 segments
                    case 2:
                        answer++;
                        break;
                    // Digit 7 has 3 segments
                    case 3:
                        answer++;
                        break;
                    // Digit 4 has 4 segments
                    case 4:
                        answer++;
                        break;
                    // Digit 8 has 7 segments
                    case 7:
                        answer++;
                        break;
                }
            }
        }
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        var answer = 0;

        foreach (var content in _input)
        {
            string[] knownSignals = new string[10];

            var change = true;
            while (change)
            {
                change = false;
                foreach (var signalInformation in content.left)
                {
                    if (knownSignals.Contains(signalInformation))
                    {
                        continue;
                    }

                    if (DeductKnowSignal(knownSignals, signalInformation))
                    {
                        change = true;
                    }

                }
            }

            Assert.DoesNotContain(null, knownSignals);

            // Make mapping for decoding
            var decodeMap = CreateDecodeMap(knownSignals);

            // Map the encoded segment values, by decoding them, and matching them to existing segments
            StringBuilder builder = new();
            foreach (var segmentInformation in content.right)
            {
                var decodedSegment = DecodeSegments(segmentInformation, decodeMap);
                builder.Append(_digitSegments.IndexOf(decodedSegment));
            }
            answer += int.Parse(builder.ToString());
        }
        return $"Answer 2: {answer}";
    }

    /// <summary>
    /// Take the signal information and try to map it using all the know signals so far
    /// </summary>
    /// <param name="knownSignals">string{}</param>
    /// <param name="signalInformation">string</param>
    /// <returns>boolean if something was found</returns>
    private static bool DeductKnowSignal(string[] knownSignals, string signalInformation)
    {
        switch (signalInformation.Length)
        {
            // Digit 1 has 2 segments
            case 2:
                knownSignals[1] = signalInformation;
                return  true;
            // Digit 7 has 3 segments
            case 3:
                knownSignals[7] = signalInformation;
                return true;
            // Digit 4 has 4 segments
            case 4:
                knownSignals[4] = signalInformation;
                return true;
            // Digit 8 has 7 segments
            case 7:
                knownSignals[8] = signalInformation;
                return true;
            // Digits 2,3,5 all have 5 segments
            case 5:
                // If there are two similarities between 1 and our signal it's 3
                if (knownSignals[1] != null)
                {
                    if (knownSignals[1].Union(signalInformation).Length == 2)
                    {
                        knownSignals[3] = signalInformation;
                        return true;
                    }
                }
                // If there are three similarities between 7 and our signal it's 3
                if (knownSignals[7] != null)
                {
                    if (knownSignals[7].Union(signalInformation).Length == 3)
                    {
                        knownSignals[3] = signalInformation;
                        return true;
                    }
                }
                // If there are two similarities between 4 and our signal it's 2
                if (knownSignals[4] != null)
                {
                    if (knownSignals[4].Union(signalInformation).Length == 2)
                    {
                        knownSignals[2] = signalInformation;
                        return true;
                    }
                    if (knownSignals[4].Union(signalInformation).Length == 3)
                    {
                        knownSignals[5] = signalInformation;
                        return true;
                    }
                }
                break;
            // Digits 0,6,9 all have 6 segments
            case 6:
                // If there is one similarity between 1 and our signal it's 6
                if (knownSignals[1] != null)
                {
                    if (knownSignals[1].Union(signalInformation).Length == 1)
                    {
                        knownSignals[6] = signalInformation;
                        return true;
                    }
                }
                // If there are 2 similarities between 7 and our signal it's 6
                if (knownSignals[7] != null)
                {
                    if (knownSignals[7].Union(signalInformation).Length == 2)
                    {
                        knownSignals[6] = signalInformation;
                        return true;
                    }
                }
                // If there are two similarities between 4 and our signal it's 2
                if (knownSignals[4] != null)
                {
                    if (knownSignals[4].Union(signalInformation).Length == 4)
                    {
                        knownSignals[9] = signalInformation;
                        return true;
                    }
                }

                if (knownSignals[0] != null && knownSignals[6] != null)
                {
                    knownSignals[9] = signalInformation;
                    return true;
                }
                if (knownSignals[0] != null && knownSignals[9] != null)
                {
                    knownSignals[6] = signalInformation;
                    return true;
                }
                if (knownSignals[9] != null && knownSignals[6] != null)
                {
                    knownSignals[0] = signalInformation;
                    return true;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// Create a decode map by looking at all the known signals and deducting the segments
    /// </summary>
    /// <param name="knownSignals">IReadOnlyList with known signals</param>
    /// <returns>char array for the decoding</returns>
    private static char[] CreateDecodeMap(IReadOnlyList<string> knownSignals)
    {
        // Make mapping
        var decodeMap = new char[7];


        // When you look what 7 has more than 1, you know what the encoded 'a' segment is
        var aEncoded = knownSignals[7].Difference(knownSignals[1])[0];
        decodeMap[(byte)aEncoded - (byte)'a'] = 'a';

        // When you look what 9 has more than 3, you know what the encoded 'b' segment is
        var bEncoded = knownSignals[9].Difference(knownSignals[3])[0];
        decodeMap[(byte)bEncoded - (byte)'a'] = 'b';

        // When you look what 8 has more than 6, you know what the encoded 'c' segment is
        var cEncoded = knownSignals[8].Difference(knownSignals[6])[0];
        decodeMap[(byte)cEncoded - (byte)'a'] = 'c';

        // When you look what 8 has more than 0, you know what the encoded 'd' segment is
        var dEncoded = knownSignals[8].Difference(knownSignals[0])[0];
        decodeMap[(byte)dEncoded - (byte)'a'] = 'd';

        // When you look what 8 has more than 9, you know what the encoded 'e' segment is
        var eEncoded = knownSignals[8].Difference(knownSignals[9])[0];
        decodeMap[(byte)eEncoded - (byte)'a'] = 'e';

        // When you look what 8 (minus segment b) has more than 3, you know what the encoded 'f' segment is
        var fEncoded = knownSignals[8].RemoveChars(bEncoded).Difference(knownSignals[2])[0];
        decodeMap[(byte)fEncoded - (byte)'a'] = 'f';

        // When you look what 9 (minus segment a) has more than 4, you know what the encoded 'g' segment is
        var gEncoded = knownSignals[9].RemoveChars(aEncoded).Difference(knownSignals[4])[0];
        decodeMap[(byte)gEncoded - (byte)'a'] = 'g';

        return decodeMap;
    }

    /// <summary>
    /// Decode the supplied segments to normal values
    /// </summary>
    /// <param name="encodedSegments">string</param>
    /// <param name="decodeMap">Map to decode</param>
    /// <returns>Decoded and normalized segment information</returns>
    private static string DecodeSegments(string encodedSegments, IReadOnlyList<char> decodeMap)
    {
        var aCharArray = encodedSegments.ToCharArray();
        for (int i = 0; i < aCharArray.Length; i++)
        {
            aCharArray[i] = decodeMap[(byte)aCharArray[i] - (byte)'a'];
        }
        return string.Join("", aCharArray).Order();
    }
}