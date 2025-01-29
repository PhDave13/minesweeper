using Minesweeper.Models;
using Xunit;

namespace Minesweeper.Tests;

public class PositionTests
{
    [Theory]
    [InlineData(0, 0, "A8")]
    [InlineData(7, 7, "H1")]
    [InlineData(0, 7, "H8")]
    [InlineData(7, 0, "A1")]
    public void ToChessNotation_ValidPositions_ReturnsCorrectNotation(int row, int column, string expected)
    {
        // Arrange
        var position = new Position(row, column);

        // Act
        var result = position.ToChessNotation();

        // Assert
        Assert.True(result.IsError is false);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(8, 0)]
    [InlineData(0, 8)]
    public void ToChessNotation_InvalidPositions_ReturnsError(int row, int column)
    {
        // Arrange
        var position = new Position(row, column);

        // Act
        var result = position.ToChessNotation();

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Position.Invalid", result.FirstError.Code);
    }

    [Theory]
    [InlineData("A8", 0, 0)]
    [InlineData("H1", 7, 7)]
    [InlineData("H8", 0, 7)]
    [InlineData("A1", 7, 0)]
    public void FromChessNotation_ValidNotation_ReturnsCorrectPosition(string notation, int expectedRow, int expectedColumn)
    {
        // Act
        var result = Position.FromChessNotation(notation);

        // Assert
        Assert.True(result.IsError is false);
        Assert.Equal(expectedRow, result.Value.Row);
        Assert.Equal(expectedColumn, result.Value.Column);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("A0")]
    [InlineData("A9")]
    [InlineData("I1")]
    [InlineData("12")]
    public void FromChessNotation_InvalidNotation_ReturnsError(string notation)
    {
        // Act
        var result = Position.FromChessNotation(notation);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Position.InvalidNotation", result.FirstError.Code);
    }
}