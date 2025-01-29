using ErrorOr;
using Minesweeper.Models;

namespace Minesweeper.Interfaces;

public interface IGameBoard
{
    int Size { get; }
    bool HasMine(Position position);
    bool IsValidPosition(Position position);
    ErrorOr<Success> Initialize(int numberOfMines);
}