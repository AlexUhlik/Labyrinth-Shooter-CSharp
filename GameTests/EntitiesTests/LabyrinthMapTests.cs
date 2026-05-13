using GameCore.Map;
using GameCore;
using System;
using Xunit;

namespace GameTests.EntitiesTests
{
    /// <summary>
    /// Набор тестов для проверки логики работы с картой лабиринта, включая преобразование координат и проверку проходимости.
    /// </summary>
    public class LabyrinthMapTests
    {
        /// <summary>
        /// Проверяет, что конструктор корректно инициализирует свойства ширины и высоты на основе переданной сетки.
        /// </summary>
        [Fact]
        public void Constructor_ValidGrid_InitializesDimensionsCorrectly()
        {
            // Arrange
            var grid = new TileType[10, 15];

            // Act
            var map = new LabyrinthMap(grid);

            // Assert
            Assert.Equal(10, map.Width());
            Assert.Equal(15, map.Height());
            Assert.NotNull(map.Grid);
        }

        /// <summary>
        /// Проверяет, что конструктор выбрасывает исключение <see cref="ArgumentNullException"/> при передаче пустой сетки.
        /// </summary>
        [Fact]
        public void Constructor_GridIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LabyrinthMap(null));
        }

        /// <summary>
        /// Проверяет корректность определения стены для тайла с типом <see cref="TileType.Wall"/>.
        /// </summary>
        [Fact]
        public void IsWall_WallTile_ReturnsTrue()
        {
            // Arrange
            var grid = new TileType[3, 3];
            grid[1, 1] = TileType.Wall;
            var map = new LabyrinthMap(grid);

            // Act & Assert
            Assert.True(map.IsWall(1, 1));
        }

        /// <summary>
        /// Проверяет, что пустой тайл не идентифицируется как стена.
        /// </summary>
        [Fact]
        public void IsWall_EmptyTile_ReturnsFalse()
        {
            // Arrange
            var grid = new TileType[3, 3];
            grid[1, 1] = TileType.Empty;
            var map = new LabyrinthMap(grid);

            // Act & Assert
            Assert.False(map.IsWall(1, 1));
        }

        /// <summary>
        /// Проверяет, что при попытке доступа к координатам вне границ сетки метод IsWall возвращает <see langword="true"/> (считает границы непроходимыми).
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(5, 0)]
        [InlineData(0, -1)]
        [InlineData(0, 5)]
        public void IsWall_CoordinatesOutOfBounds_ReturnsTrue(int x, int y)
        {
            // Arrange
            var grid = new TileType[5, 5];
            var map = new LabyrinthMap(grid);

            // Act & Assert
            Assert.True(map.IsWall(x, y));
        }

        /// <summary>
        /// Проверяет корректность преобразования мировых координат в индексы тайлов сетки.
        /// </summary>
        [Fact]
        public void ConvertToTileCoordinates_WorldPosition_CalculatesCorrectIndices()
        {
            // Arrange
            var grid = new TileType[10, 10];
            var map = new LabyrinthMap(grid);
            var position = new Point(120f, 60f);

            // Act
            var tileCoords = map.ConvertToTileCoordinates(position);

            // Assert
            Assert.Equal(2, (int)tileCoords.X);
            Assert.Equal(1, (int)tileCoords.Y);
        }

        /// <summary>
        /// Проверяет корректность преобразования индексов тайла в мировые координаты центра этого тайла.
        /// </summary>
        [Fact]
        public void ConvertToWorldCoordinates_GridIndices_ReturnsCenterOfTile()
        {
            // Arrange
            var grid = new TileType[10, 10];
            var map = new LabyrinthMap(grid);
            int gridX = 1;
            int gridY = 2;

            // Act
            var worldPos = map.ConvertToWorldCoordinates(gridX, gridY);

            // Assert
            Assert.Equal(75f, worldPos.X);
            Assert.Equal(125f, worldPos.Y);
        }
    }
}