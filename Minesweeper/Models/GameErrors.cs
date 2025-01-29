using ErrorOr;

namespace Minesweeper.Models;

public static class GameErrors
{
    public static class Position
    {
        public static Error InvalidPosition => Error.Validation(
            code: "Position.Invalid",
            description: "The position is outside the valid range.");

        public static Error InvalidNotation => Error.Validation(
            code: "Position.InvalidNotation",
            description: "Invalid chess notation format or range.");
    }

    public static class Movement
    {
        public static Error InvalidMove => Error.Validation(
            code: "Movement.Invalid",
            description: "Cannot move outside the board.");

        public static Error GameOver => Error.Validation(
            code: "Movement.GameOver",
            description: "The game is already over.");
    }

    public static class Board
    {
        public static Error TooManyMines => Error.Validation(
            code: "Board.TooManyMines",
            description: "Too many mines for the board size.");
    }
}