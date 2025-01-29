using ErrorOr;

namespace Minesweeper.Models;

public record Position(int Row, int Column)
{
    //Should use constants instead of inline values.
    public ErrorOr<string> ToChessNotation()
    {
        if (Row < 0 || Row > 7 || Column < 0 || Column > 7)
        {
            return GameErrors.Position.InvalidPosition;
        }
            
        char file = (char)('A' + Column);
        int rank = 8 - Row;
        return $"{file}{rank}";
    }

    public static ErrorOr<Position> FromChessNotation(string notation)
    {
        if (string.IsNullOrEmpty(notation) || notation.Length != 2)
        {
            return GameErrors.Position.InvalidNotation;
        }

        char file = char.ToUpper(notation[0]);
        if (!int.TryParse(notation[1].ToString(), out int rank))
        {
            return GameErrors.Position.InvalidNotation;
        }

        if (file < 'A' || file > 'H' || rank < 1 || rank > 8)
        {
            return GameErrors.Position.InvalidNotation;
        }

        int column = file - 'A';
        int row = 8 - rank;

        return new Position(row, column);
    }
}