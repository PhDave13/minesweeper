using ErrorOr;
using Minesweeper.Interfaces;
using Minesweeper.Models;

namespace Minesweeper.Engine;

public class Player : IPlayer
{
    private readonly IGameBoard _board;
    public Position CurrentPosition { get; private set; }
    public int Lives { get; private set; }
    public int Moves { get; private set; }
    public bool IsAlive => Lives > 0;

    public Player(IGameBoard board, Position startPosition, int initialLives)
    {
        _board = board ?? throw new ArgumentNullException(nameof(board));
        
        if (!board.IsValidPosition(startPosition))
        {
            throw new ArgumentException("Invalid start position");
        }

        CurrentPosition = startPosition;
        Lives = initialLives;
        Moves = 0;
    }

    public ErrorOr<Success> Move(Direction direction)
    {
        var newPosition = direction switch
        {
            Direction.Up => CurrentPosition with { Row = CurrentPosition.Row - 1 },
            Direction.Down => CurrentPosition with { Row = CurrentPosition.Row + 1 },
            Direction.Left => CurrentPosition with { Column = CurrentPosition.Column - 1 },
            Direction.Right => CurrentPosition with { Column = CurrentPosition.Column + 1 },
            _ => CurrentPosition
        };

        if (!_board.IsValidPosition(newPosition))
        {
            return GameErrors.Movement.InvalidMove;
        }

        CurrentPosition = newPosition;
        Moves++;

        if (_board.HasMine(CurrentPosition))
        {
            TakeDamage();
        }

        return Result.Success;
    }

    public void TakeDamage()
    {
        Lives--;
    }
}