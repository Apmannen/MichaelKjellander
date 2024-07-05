namespace MichaelKjellander.Api.Features.Sudoku;

public class SudokuBoard
{
    private const int SideLength = 9;
    public const int TotalNumSquares = SideLength * SideLength;
    private const int GroupSize = 3;
    private readonly SudokuSquare[] _squares = new SudokuSquare[TotalNumSquares];
    private int _bruteForceSafetyCount;
    private bool _isSolvable;

    public SudokuBoard()
    {
        for (int i = 0; i < _squares.Length; i++)
        {
            _squares[i] = new SudokuSquare { Index = i };
        }

        for (int y = 0; y < SideLength; y++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                SudokuSquare square = Get(x, y);
                square.X = x;
                square.Y = y;

                int xGroup = (int)Math.Floor((float)x / (float)GroupSize);
                int yGroup = (int)Math.Floor((float)y / (float)GroupSize);
                square.Group = xGroup + "" + yGroup;

                if (square.HasValue)
                {
                    square.Possibles.Add(square.Value);
                }
            }
        }
    }
    
    public static SudokuBoard CreateBoardFromValues(int[] values)
    {
        SudokuBoard board = new SudokuBoard();
        int x = 0;
        int y = 0;
        for (int i = 0; i < values.Length; i++)
        {
            SetSquareValue(board.Get(i), values[i]);
        }

        return board;
    }

    public int[] Values
    {
        get { return _squares.Select(s => s.Value).ToArray(); }
    }

    public void Solve()
    {
        _isSolvable = true;
        bool anyChange = true;
        while (anyChange)
        {
            anyChange = false;
            foreach (SudokuSquare square in _squares)
            {
                anyChange = TrySolveSquare(square) || anyChange;
            }

            if (IsSolved)
            {
                break;
            }

            if (anyChange)
            {
                continue;
            }

            foreach (SudokuSquare square in _squares)
            {
                anyChange = TryFindUniquePossible(square);
                if (anyChange)
                {
                    break;
                }
            }
        }

        foreach (SudokuSquare square in _squares)
        {
            if (square.Possibles.Count == 0)
            {
                _isSolvable = false;
                break;
            }

            if (!square.HasValue)
            {
                continue;
            }

            List<SudokuSquare> otherSquares = _squares.Where(s => s.Index != square.Index)
                .Where(s => s.X == square.X || s.Y == square.Y || s.Group == square.Group).ToList();
            if (otherSquares.FirstOrDefault(s => s.Value == square.Value) != null)
            {
                _isSolvable = false;
                break;
            }
        }

        if (!IsSolved && _isSolvable)
        {
            BruteForce();
        }

        if (!IsSolved)
        {
            _isSolvable = false;
        }
    }

    private void BruteForce()
    {
        _bruteForceSafetyCount = 0;
        SudokuSquare first = _squares.First(s => !s.HasValue);
        bool didSucceed = TryBruteForceSingle(first, 0);
        if (!didSucceed)
        {
            return;
        }

        foreach (SudokuSquare square in _squares)
        {
            if (square.HasValue)
            {
                continue;
            }

            square.Value = square.TmpValue;
            square.TmpValue = 0;
        }
    }

    private bool TryBruteForceSingle(SudokuSquare square, int possibleIndex)
    {
        if (possibleIndex >= (square.Possibles.Count))
        {
            return false;
        }

        if (_bruteForceSafetyCount++ > 100000)
        {
            return false;
        }

        int preliminaryValue = square.Possibles[possibleIndex];
        bool isValidValue = _squares
            .FirstOrDefault(s => s.Index != square.Index &&
                                 (s.Value == preliminaryValue || s.TmpValue == preliminaryValue) &&
                                 (s.X == square.X || s.Y == square.Y || s.Group == square.Group)) == null;
        if (isValidValue)
        {
            square.TmpValue = preliminaryValue;
            SudokuSquare? nextSquare = null;
            for (int i = square.Index + 1; i < TotalNumSquares; i++)
            {
                if (_squares[i].Value == 0)
                {
                    nextSquare = _squares[i];
                    break;
                }
            }

            if (nextSquare == null)
            {
                return true;
            }

            bool didSucceed = TryBruteForceSingle(nextSquare, 0);
            if (!didSucceed)
            {
                square.TmpValue = 0;
                return TryBruteForceSingle(square, possibleIndex + 1);
            }

            return didSucceed;
        }
        else
        {
            square.TmpValue = 0;
            return TryBruteForceSingle(square, possibleIndex + 1);
        }
    }

    public bool IsSolved => _squares.FirstOrDefault(s => !s.HasValue) == null;

    private bool TryFindUniquePossible(SudokuSquare square)
    {
        if (square.HasValue)
        {
            return false;
        }

        List<SudokuSquare> otherSquares = _squares.Where(s => s.Index != square.Index).ToList();
        if (TryFindUniquePossible(square,
                otherSquares.Where(s => s.X == square.X || s.Y == square.Y || s.Group == square.Group).ToList()))
        {
            return true;
        }

        if (TryFindUniquePossible(square, otherSquares.Where(s => s.X == square.X).ToList()))
        {
            return true;
        }

        if (TryFindUniquePossible(square, otherSquares.Where(s => s.Y == square.Y).ToList()))
        {
            return true;
        }

        if (TryFindUniquePossible(square, otherSquares.Where(s => s.Group == square.Group).ToList()))
        {
            return true;
        }

        return false;
    }

    private bool TryFindUniquePossible(SudokuSquare square, List<SudokuSquare> otherSquares)
    {
        HashSet<int> othersPossibles = [];
        foreach (SudokuSquare otherSquare in otherSquares)
        {
            othersPossibles.UnionWith(otherSquare.Possibles);
        }

        HashSet<int> uniquePossibles = [];
        uniquePossibles.UnionWith(square.Possibles);
        uniquePossibles.RemoveWhere(possibleValue => othersPossibles.Contains(possibleValue));

        if (uniquePossibles.Count != 1)
        {
            return false;
        }

        SetSquareValue(square, uniquePossibles.First());
        return true;
    }


    private bool TrySolveSquare(SudokuSquare square)
    {
        if (square.HasValue)
        {
            return false;
        }

        HashSet<int> possibles = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        HashSet<int> impossibles = [];
        List<SudokuSquare> otherSquaresWithValues =
            _squares.Where(s => s.Index != square.Index && s.Value != 0).ToList();

        impossibles.UnionWith(otherSquaresWithValues
            .Where(s => s.X == square.X || s.Y == square.Y || s.Group == square.Group)
            .Select(s => s.Value));

        possibles.RemoveWhere(value => impossibles.Contains(value));
        if (possibles.Count == 0)
        {
            throw new Exception("Illegal count");
        }

        square.Possibles.Clear();
        square.Possibles.AddRange(possibles);

        if (possibles.Count != 1)
        {
            return false;
        }

        SetSquareValue(square, possibles.First());
        return true;
    }

    public SudokuSquare Get(int index)
    {
        return _squares[index];
    }

    public SudokuSquare Get(int x, int y)
    {
        int index = x + (y * SideLength);
        return _squares[index];
    }

    public void Debug()
    {
        Console.WriteLine("------");
        for (int y = 0; y < SideLength; y++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                SudokuSquare square = Get(x, y);
                Console.Write(square.TmpValue != 0 ? square.TmpValue : square.Value);
            }

            Console.WriteLine();
        }
    }

    private static void SetSquareValue(SudokuSquare square, int newValue)
    {
        square.Value = newValue;
        if (newValue != 0)
        {
            square.Possibles.Clear();
            square.Possibles.Add(newValue);
        }
    }
}