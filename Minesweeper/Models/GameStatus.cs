namespace Minesweeper.Models;

public record GameStatus(
    string Position,
    int LivesRemaining,
    int MovesTaken,
    bool IsGameOver,
    bool HasWon
);