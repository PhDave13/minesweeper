using Minesweeper.Engine;
using Minesweeper.Models;
using Xunit;

namespace Minesweeper.Tests;

public class GameBoardTests
{
    [Fact]
    public void Initialize_WithValidNumberOfMines_PlacesCorrectNumberOfMines()
    {
        // Arrange
        var board = new GameBoard(8);
        const int numberOfMines = 10;

        // Act
        var result = board.Initialize(numberOfMines);
        
        // Assert
        Assert.False(result.IsError);
        
        // Count mines
        int mineCount = 0;
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.HasMine(new Position(row, col)))
                {
                    mineCount++;
                }
            }
        }
        
        Assert.Equal(numberOfMines, mineCount);
    }

    [Fact]
    public void Initialize_WithTooManyMines_ReturnsError()
    {
        // Arrange
        var board = new GameBoard(8);
        int tooManyMines = 65; // 8x8 = 64 cells

        // Act
        var result = board.Initialize(tooManyMines);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Board.TooManyMines", result.FirstError.Code);
    }

    [Theory]
    [InlineData(0, 0, true)]  // Top-left corner
    [InlineData(7, 7, true)]  // Bottom-right corner
    [InlineData(3, 3, true)]  // Middle of board
    [InlineData(-1, 0, false)] // Outside left
    [InlineData(0, -1, false)] // Outside top
    [InlineData(8, 0, false)]  // Outside bottom
    [InlineData(0, 8, false)]  // Outside right
    public void IsValidPosition_ReturnsExpectedResult(int row, int column, bool expected)
    {
        // Arrange
        var board = new GameBoard(8);
        var position = new Position(row, column);

        // Act
        var result = board.IsValidPosition(position);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void HasMine_WithInvalidPosition_ReturnsFalse()
    {
        // Arrange
        var board = new GameBoard(8);
        var invalidPosition = new Position(-1, -1);

        // Act
        var result = board.HasMine(invalidPosition);

        // Assert
        Assert.False(result);
    }
}