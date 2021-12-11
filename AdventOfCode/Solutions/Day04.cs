namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/4
/// </summary>
public class Day04 : AdventOfCodeBase
{
    private readonly List<int> _drawnNumbers;
    private readonly List<Board> _boards = new();
    private readonly List<Board> _playedBoards;

    public Day04()
    {
        Assert.True(File.Exists(this.InputFilename));

        var lines = File.ReadAllLines(this.InputFilename).ToList();

        _drawnNumbers = lines[0].SplitClean(',').Select(int.Parse).ToList();

        var boardLines = new List<string>();
        for (var index = 2; index < lines.Count; index++)
        {
            var line = lines[index];
            if (string.IsNullOrEmpty(line))
            {
                if (boardLines.Count > 0)
                {
                    _boards.Add(new Board(boardLines));
                    boardLines.Clear();
                }
                continue;
            }
            boardLines.Add(line);
        }

        if (boardLines.Count > 0)
        {
            _boards.Add(new Board(boardLines));
        }

        _playedBoards = PlayBoards();
    }

    private List<Board> PlayBoards()
    {
        var bingoBoards = new List<Board>();
        foreach (var drawnNumber in _drawnNumbers)
        {
            foreach (var board in _boards)
            {
                if (board.HasBingo())
                {
                    continue;
                }
                board.DrawNumber(drawnNumber);
                if (board.HasBingo())
                {
                    bingoBoards.Add(board);
                }
            }
        }

        return bingoBoards;
    }

    public override string AnswerPartOne()
    {
        var winningBoard = _playedBoards.First();
        return $"Answer 1: {winningBoard.Score()}";
    }

    public override string AnswerPartTwo()
    {
        var winningBoard = _playedBoards.Last();
        return $"Answer 2: {winningBoard.Score()}";
    }
}


public class Board
{
    private bool _hasBingo;

    public Board(IReadOnlyList<string> rows)
    {
        for (int i = 0; i < rows.Count; i++)
        {
            var processedRows = rows[i].SplitClean(' ');
            if (Numbers == null)
            {
                
                Numbers = new Grid<int>(rows.Count, processedRows.Length);
                WasDrawn = new Grid<bool>(rows.Count, processedRows.Length);
            }
            for (int j = 0; j < processedRows.Length; j++)
            {
                Numbers[i, j] = int.Parse(processedRows[j]);
            }
        }
    }

    public bool HasBingo()
    {
        if (_hasBingo)
        {
            return true;
        }
        for (int y = 0; y < 5; y++)
        {
            bool horizontal = true;
            bool vertical = true;
            for (int x = 0; x < 5; x++)
            {
                horizontal &= WasDrawn[y, x];
                vertical &= WasDrawn[x, y];
            }

            if (vertical || horizontal)
            {
                _hasBingo = true;
            }
        }

        return _hasBingo;
    }

    private int LastDrawnNumber { get; set; }
    private Grid<int> Numbers { get; }
    private Grid<bool> WasDrawn { get; }

    public void DrawNumber(int number)
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (Numbers[x, y] == number)
                {
                    WasDrawn[x, y] = true;
                }
            }
        }

        LastDrawnNumber = number;
    }

    private int Sum()
    {
        int sum = 0;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!WasDrawn[x, y])
                {
                    sum += Numbers[x, y];
                }
            }
        }

        return sum;
    }

    public int Score() => Sum() * LastDrawnNumber;
}
