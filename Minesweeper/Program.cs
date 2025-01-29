using Minesweeper.Engine;
using Minesweeper.Models;

bool playAgain;
do
{
    // Initialize board.  In future versions, we can make this interactive where the user chooses these values.
    const int boardSize = 8;
    const int numberOfMines = 10;
    const int initialLives = 3;

    var gameBoard = new GameBoard(boardSize);
    gameBoard.Initialize(numberOfMines);

    // Start player at the bottom row, D or E Rank
    var startPosition = new Position(boardSize - 1, boardSize / 2);
    var player = new Player(gameBoard, startPosition, initialLives);

    var gameEngine = new GameEngine(gameBoard, player);
    var consoleHandler = new ConsoleGameHandler(gameEngine);
    playAgain = consoleHandler.Run();

} while (playAgain);

Console.WriteLine("\nThanks for playing! Press any key to exit...");
Console.ReadKey(true);