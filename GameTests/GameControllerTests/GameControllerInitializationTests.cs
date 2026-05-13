using Xunit;
using Application.Game;
using GameCore.Map;
using System.Linq;

namespace GameTests.GameControllerTests
{
    /// <summary>
    /// Тесты инициализации и начального состояния игрового контроллера.
    /// </summary>
    public class GameControllerInitializationTests
    {
        /// <summary>
        /// Проверяет, что при создании контроллера игроки инициализируются, 
        /// не являются null и корректно добавляются в общий список игровых объектов.
        /// </summary>
        [Fact]
        public void Constructor_NewInstance_InitializesPlayersAndGameObjects()
        {
            // Arrange
            var grid = new TileType[11, 11];
            var map = new LabyrinthMap(grid);

            // Act
            var controller = new GameController(map);

            // Assert
            Assert.NotNull(controller.Player1);
            Assert.NotNull(controller.Player2);
            Assert.False(controller.IsGameOver);

            var gameObjects = controller.GameObjects;
            Assert.Contains(controller.Player1, gameObjects);
            Assert.Contains(controller.Player2, gameObjects);
        }

        /// <summary>
        /// Проверяет, что при создании контроллера уровень автоматически 
        /// заполняется начальным количеством врагов согласно константе TargetEnemyCount.
        /// </summary>
        [Fact]
        public void Constructor_NewInstance_FillsLevelWithInitialEnemies()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[15, 15]);

            // Act
            var controller = new GameController(map);

            // Assert
            Assert.Equal(15, controller.Enemies.Count);
        }

        /// <summary>
        /// Проверяет, что при инициализации списки активных снарядов и бонусов пусты.
        /// </summary>
        [Fact]
        public void Constructor_NewInstance_ListsShouldBeEmpty()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[10, 10]);

            // Act
            var controller = new GameController(map);

            // Assert
            Assert.Empty(controller.ActiveBullets);
            Assert.Empty(controller.ActivePrizes);
        }
    }
}