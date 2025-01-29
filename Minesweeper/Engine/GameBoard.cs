using ErrorOr;
using Minesweeper.Interfaces;
using Minesweeper.Models;

namespace Minesweeper.Engine;

public class GameBoard : IGameBoard
{
    private readonly bool[,] _mines;
    private readonly Random _random;

    public int Size { get; }

    public GameBoard(int size = 8, Random? random = null)
    {
        Size = size;
        _mines = new bool[size, size];
        _random = random ?? new Random();
    }

    public bool HasMine(Position position)
    {
        return IsValidPosition(position) && _mines[position.Row, position.Column];
    }

    public bool IsValidPosition(Position position)
    {
        return position.Row >= 0 && position.Row < Size &&
               position.Column >= 0 && position.Column < Size;
    }

    public ErrorOr<Success> Initialize(int numberOfMines)
    {
        if (numberOfMines >= Size * Size)
        {
            return GameErrors.Board.TooManyMines;
        }

        // Clear the board
        Array.Clear(_mines, 0, _mines.Length);

        // Place mines randomly
        int minesPlaced = 0;
        while (minesPlaced < numberOfMines)
        {
            int row = _random.Next(Size);
            int col = _random.Next(Size);

            if (!_mines[row, col])
            {
                _mines[row, col] = true;
                minesPlaced++;
            }
        }

        return Result.Success;
    }
}