namespace AdventOfCode.Solutions;

public class Day03 : AdventOfCodeBase
{
    private readonly List<string> _diagnosticsReport;

    public Day03()
    {
        Assert.True(File.Exists(this.InputFilename));
        _diagnosticsReport = File.ReadAllLines(this.InputFilename).Where(l => !string.IsNullOrEmpty(l)).ToList();
    }

    public override string AnswerPartOne()
    {
        int nrOfDiagnosticsValues = 0;
        int gamma = 0;
        int epsilon = 0;

        var bitCounts = new List<int>();
        // Prepare bit counts array
        var firstValue = _diagnosticsReport[0];
        foreach (var c in firstValue.AsSpan())
        {
            if (char.IsDigit(c))
            {
                bitCounts.Add(0);
            }
        }

        // count bits
        foreach (var diagnosticValue in _diagnosticsReport)
        {
            nrOfDiagnosticsValues++;
            var bits = diagnosticValue.AsSpan();
            int bitCountIndex= 0;
            foreach (var bit in bits)
            {
                switch (bit)
                {
                    case '0':
                        bitCountIndex++;
                        break;
                    case '1':
                        bitCounts[bitCountIndex++]++;
                        break;
                }
            }
        }

        // Generate gamma and epsilon
        int bitIndex = 1;
        for (var index = bitCounts.Count - 1; index >= 0; index--)
        {
            var bitCount = bitCounts[index];
            if (bitCount > nrOfDiagnosticsValues >> 1)
            {
                gamma |= bitIndex;
            }
            else
            {
                epsilon |= bitIndex;
            }
            bitIndex <<= 1;
        }

        return $"Answer 1: {gamma * epsilon}";
    }
}