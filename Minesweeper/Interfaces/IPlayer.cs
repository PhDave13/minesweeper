using ErrorOr;
using Minesweeper.Models;

namespace Minesweeper.Interfaces;

public interface IPlayer
{
    Position CurrentPosition { get; }
    int Lives { get; }
    int Moves { get; }
    bool IsAlive { get; }
    ErrorOr<Success> Move(Direction direction);
    void TakeDamage();
}