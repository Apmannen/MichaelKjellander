namespace MichaelKjellander.Api.Features.Sudoku;

//Just a data class, doesn't control its own logic
public class SudokuSquare
{
    public int Value;
    public int TmpValue;
    public int Index;
    public int X;
    public int Y;
    public string Group = "";
    public readonly List<int> Possibles = [];
    public bool HasValue => Value != 0;

    public override string ToString()
    {
        return $"Square[Value={Value}, X={X}, Y={Y}, Group={Group}]";
    }
}