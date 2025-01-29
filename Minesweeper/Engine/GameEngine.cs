using ErrorOr;
using Minesweeper.Interfaces;
using Minesweeper.Models;

namespace Minesweeper.Engine;

public class GameEngine
{
    private readonly IGameBoard _board;
    private readonly IPlayer _player;
    private readonly int _boardSize;
    private bool _isGameOver;

    public GameEngine(IGameBoard board, IPlayer player)
    {
        _board = board;
        _player = player;
        _boardSize = board.Size;
    }

    private bool HasPlayerWon() => _player.CurrentPosition.Row == 0;

    public bool IsGameOver() => _isGameOver || !_player.IsAlive || HasPlayerWon();

    public ErrorOr<Success> ProcessMove(Direction direction)
    {
        if (IsGameOver())
        {
            return GameErrors.Movement.GameOver;
        }

        var moveResult = _player.Move(direction);
        if (moveResult.IsError)
        {
            return moveResult.Errors;
        }

        _isGameOver = !_player.IsAlive || HasPlayerWon();
        return Result.Success;
    }

    public ErrorOr<GameStatus> GetGameStatus()
    {
        var positionResult = _player.CurrentPosition.ToChessNotation();
        if (positionResult.IsError)
        {
            return positionResult.Errors;
        }

        return new GameStatus(
            positionResult.Value,
            _player.Lives,
            _player.Moves,
            IsGameOver(),
            HasPlayerWon()
        );
    }
}
