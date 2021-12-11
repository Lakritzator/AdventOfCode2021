namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/11
/// </summary>
public class Day11 : AdventOfCodeBase
{
    private Grid<int> _octoGrid;

    public Day11()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        // = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        _octoGrid = new Grid<int>(lines[0].Length, lines.Length);

        for (int y = 0; y < _octoGrid.Height; y++)
        {
            var depths = lines[y].AsSpan();
            for (int x = 0; x < _octoGrid.Width; x++)
            {
                _octoGrid[x, y] = depths[x] - '0';
            }
        }
    }

    public override string AnswerPartOne()
    {
        // Count flashes over 100 steps
        int answer = 0;

        var octoGrid = _octoGrid.Clone();
        int step = 0;
        while (step < 100)
        {
            answer += Step(octoGrid, out _);
            step++;
        }
        return $"Answer 1: {answer}";
    }

    public override string AnswerPartTwo()
    {
        var octoGrid = _octoGrid.Clone();
        int step = 0;
        bool didAllOctosFlash;
        do
        {
            Step(octoGrid, out var octosWhichFlashed);
            step++;
            didAllOctosFlash = octosWhichFlashed.All(p => octosWhichFlashed[p]);
        } while (!didAllOctosFlash);
        return $"Answer 2: {step}";
    }

    /// <summary>
    /// Process a step
    /// </summary>
    /// <param name="octoGrid">Grid with octo energy levels as input for the step</param>
    /// <param name="octosWhichFlashed">Grid with which octos flashed (used in part two)</param>
    /// <returns>int with the number of flashes in this step</returns>
    private int Step(Grid<int> octoGrid, out Grid<bool> octosWhichFlashed)
    {
        int flashes = 0;
        var octosWhichWillFlash = new Stack<Point>();
        octosWhichFlashed = octoGrid.CreateEmptyLike<bool>();

        foreach (var point in octoGrid)
        {
            if (IncreaseEnergy(point, octoGrid, octosWhichFlashed))
            {
                // We have a flash
                octosWhichWillFlash.Push(point);
            }
        }
        var octosWhichWillFlashNext = new Stack<Point>();

        while (octosWhichWillFlash.Count > 0)
        {
            while (octosWhichWillFlash.TryPop(out var flashingOcto))
            {
                // Count a flash
                flashes++;
                foreach (var nextOctoFlashing in FlashOcto(flashingOcto, octoGrid, octosWhichFlashed))
                {
                    octosWhichWillFlashNext.Push(nextOctoFlashing);
                }
            }

            // Swap (octosWhichWillFlash should be empty, the octosWhichWillFlashNext need to be processed
            (octosWhichWillFlash, octosWhichWillFlashNext) = (octosWhichWillFlashNext, octosWhichWillFlash);
        }

        return flashes;
    }

    /// <summary>
    /// Process the octo flashing by transferring energy to the octos around it
    /// </summary>
    /// <param name="flashingOcto"></param>
    /// <param name="octoGrid"></param>
    /// <param name="octosWhichFlashed"></param>
    /// <returns>IEnumerable with octos who also flash due to transfer</returns>
    private IEnumerable<Point> FlashOcto(Point flashingOcto, Grid<int> octoGrid, Grid<bool> octosWhichFlashed)
    {
        foreach (var octoAround in flashingOcto.PointsAllAround())
        {
            if (ProcessEnergyTransfer(octoAround, octoGrid, octosWhichFlashed))
            {
                // Provide the octo with flashed to the calling code
                yield return octoAround;
            }
        }
    }

    /// <summary>
    /// Due to a flash there is an energy transfer of 1 to the octo specified
    /// </summary>
    /// <param name="octoToTransferEnergyTo">Point with the octo to transfer 1 energy to</param>
    /// <param name="octoGrid">Grid with the octo energy levels</param>
    /// <param name="octosWhichFlashed">Grid with the octos who already flashed</param>
    /// <returns>true if the octo flashed</returns>
    private bool ProcessEnergyTransfer(Point octoToTransferEnergyTo, Grid<int> octoGrid, Grid<bool> octosWhichFlashed)
    {
        return octoGrid.IsValid(octoToTransferEnergyTo) && IncreaseEnergy(octoToTransferEnergyTo, octoGrid, octosWhichFlashed);
    }

    /// <summary>
    /// Increase the energy of the specified octo with 1, check if it flashes
    /// </summary>
    /// <param name="currentOcto"></param>
    /// <param name="octoGrid">Grid with the octo energy levels</param>
    /// <param name="octosWhichFlashed">Grid with the octos who already flashed</param>
    /// <returns>true if the current octo flashed due to energy abundance</returns>
    private bool IncreaseEnergy(Point currentOcto, Grid<int> octoGrid, Grid<bool> octosWhichFlashed)
    {
        if (octosWhichFlashed[currentOcto])
        {
            return false;
        }
        var newEnergy = octoGrid[currentOcto] += 1;
        // Check for flash
        if (newEnergy <= 9)
        {
            // No flash
            return false;
        }
        // Octo energy reset to 0
        octoGrid[currentOcto] = 0;
        octosWhichFlashed[currentOcto] = true;
        return true;
    }
}