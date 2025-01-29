using Minesweeper.Engine;
using Minesweeper.Interfaces;
using Minesweeper.Models;
using NSubstitute;
using Xunit;

namespace Minesweeper.Tests;

public class PlayerTests
{
    private readonly IGameBoard _mockBoard;
    private readonly Position _startPosition = new(7, 4);
    private readonly int _initialLives = 3;

    public PlayerTests()
    {
        _mockBoard = Substitute.For<IGameBoard>();
        _mockBoard.IsValidPosition(Arg.Any<Position>()).Returns(true);
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesPlayer()
    {
        // Act
        var player = new Player(_mockBoard, _startPosition, _initialLives);

        // Assert
        Assert.Equal(_startPosition, player.CurrentPosition);
        Assert.Equal(_initialLives, player.Lives);
        Assert.Equal(0, player.Moves);
        Assert.True(player.IsAlive);
    }

    [Theory]
    [InlineData(Direction.Up, -1, 0)]
    [InlineData(Direction.Down, 1, 0)]
    [InlineData(Direction.Left, 0, -1)]
    [InlineData(Direction.Right, 0, 1)]
    public void Move_InValidDirection_UpdatesPosition(Direction direction, int rowDelta, int colDelta)
    {
        // Arrange
        var player = new Player(_mockBoard, _startPosition, _initialLives);
        var expectedPosition = new Position(
            _startPosition.Row + rowDelta,
            _startPosition.Column + colDelta);

        _mockBoard.IsValidPosition(expectedPosition).Returns(true);
        _mockBoard.HasMine(expectedPosition).Returns(false);

        // Act
        var result = player.Move(direction);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(expectedPosition, player.CurrentPosition);
        Assert.Equal(1, player.Moves);
        Assert.Equal(_initialLives, player.Lives);
    }

    [Fact]
    public void Move_OntoMine_ReducesLives()
    {
        // Arrange
        var player = new Player(_mockBoard, _startPosition, _initialLives);
        var minePosition = new Position(_startPosition.Row - 1, _startPosition.Column);
        
        _mockBoard.IsValidPosition(minePosition).Returns(true);
        _mockBoard.HasMine(minePosition).Returns(true);

        // Act
        var result = player.Move(Direction.Up);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(minePosition, player.CurrentPosition);
        Assert.Equal(_initialLives - 1, player.Lives);
        Assert.Equal(1, player.Moves);
    }

    [Fact]
    public void Move_ToInvalidPosition_ReturnsError()
    {
        // Arrange
        var player = new Player(_mockBoard, _startPosition, _initialLives);
        var invalidPosition = new Position(_startPosition.Row - 1, _startPosition.Column);
        
        _mockBoard.IsValidPosition(invalidPosition).Returns(false);

        // Act
        var result = player.Move(Direction.Up);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Movement.Invalid", result.FirstError.Code);
        Assert.Equal(_startPosition, player.CurrentPosition);
        Assert.Equal(0, player.Moves);
        Assert.Equal(_initialLives, player.Lives);
    }

    [Fact]
    public void TakeDamage_ReducesLives()
    {
        // Arrange
        var player = new Player(_mockBoard, _startPosition, _initialLives);

        // Act
        player.TakeDamage();

        // Assert
        Assert.Equal(_initialLives - 1, player.Lives);
    }

    [Fact]
    public void IsAlive_WithZeroLives_ReturnsFalse()
    {
        // Arrange
        var player = new Player(_mockBoard, _startPosition, 1);

        // Act
        player.TakeDamage();

        // Assert
        Assert.False(player.IsAlive);
    }
}