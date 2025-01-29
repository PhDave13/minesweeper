using ErrorOr;
using Minesweeper.Models;

namespace Minesweeper.Engine;

public class ConsoleGameHandler
{
    private readonly GameEngine _engine;
    private string? _errorMessage;

    public ConsoleGameHandler(GameEngine engine)
    {
        _engine = engine;
    }

    public bool Run()
    {
        Console.WriteLine("Welcome to Minesweeper!");
        Console.WriteLine("Use W/A/S/D or arrow keys to move.");
        Console.WriteLine("Try to reach the right side of the board without hitting too many mines!");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey(true);
        
        DisplayGameStatus();

        while (!_engine.IsGameOver())
        {
            ProcessInput();
        }

        return DisplayGameOver();
    }

    private void DisplayGameStatus()
    {
        Console.Clear();
        var statusResult = _engine.GetGameStatus();
        if (statusResult.IsError)
        {
            Console.WriteLine("Error displaying game status");
            return;
        }

        var status = statusResult.Value;
        Console.WriteLine("\n---------------------------");
        Console.WriteLine($"Position: {status.Position}");
        Console.WriteLine($"Lives: {status.LivesRemaining}");
        Console.WriteLine($"Moves: {status.MovesTaken}");
        Console.WriteLine("---------------------------");

        if (!string.IsNullOrEmpty(_errorMessage))
        {
            if (_errorMessage.Contains("mine"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"\n{_errorMessage}");
            Console.ResetColor();
            _errorMessage = null;
        }
    }

    private void ProcessInput()
    {
        var key = Console.ReadKey(true).Key;
        Direction? direction = key switch
        {
            ConsoleKey.W or ConsoleKey.UpArrow => Direction.Up,
            ConsoleKey.S or ConsoleKey.DownArrow => Direction.Down,
            ConsoleKey.A or ConsoleKey.LeftArrow => Direction.Left,
            ConsoleKey.D or ConsoleKey.RightArrow => Direction.Right,
            _ => null
        };

        if (direction.HasValue)
        {
            var moveResult = _engine.ProcessMove(direction.Value);
            if (moveResult.IsError)
            {
                _errorMessage = moveResult.FirstError.Description;
            }
            DisplayGameStatus();
        }
    }

    private bool DisplayGameOver()
    {
        var statusResult = _engine.GetGameStatus();
        if (statusResult.IsError)
        {
            Console.WriteLine("Error displaying final game status");
            return false;
        }

        var status = statusResult.Value;
        Console.Clear();
        
        // Set color based on game outcome
        Console.ForegroundColor = status.HasWon ? ConsoleColor.Green : ConsoleColor.Red;

        Console.WriteLine("\n===================");
        Console.WriteLine(status.HasWon 
            ? "Congratulations! You've won!" 
            : "Game Over - Better luck next time!");
        Console.WriteLine($"Final Position: {status.Position}");
        Console.WriteLine($"Final Score: {status.MovesTaken} moves");
        Console.WriteLine("===================\n");

        Console.WriteLine("Would you like to play again? (Y/N)");
        Console.ResetColor();

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y)
                return true;
            if (key == ConsoleKey.N)
                return false;
        }
    }
}