using Xunit;
using Application.Game;
using GameCore.Map;
using System.Linq;

namespace GameTests.GameControllerTests
{
    /// <summary>
    /// Тестирование обработки пользовательского ввода и его влияния на состояние игроков и игрового мира.
    /// </summary>
    public class GameControllerInputTests
    {
        /// <summary>
        /// Проверяет, что нажатие клавиши Пробел приводит к созданию пули от имени первого игрока.
        /// </summary>
        [Fact]
        public void HandleInput_SpacePressed_CreatesBulletOwnedByPlayer1()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[15, 15]);
            var controller = new GameController(map);
            int initialBulletsCount = controller.ActiveBullets.Count;

            // Act
            controller.HandleInput(GameInput.Space);

            // Assert
            Assert.Equal(initialBulletsCount + 1, controller.ActiveBullets.Count);

            var lastBullet = controller.ActiveBullets.LastOrDefault();
            Assert.NotNull(lastBullet);
            Assert.Equal(1, lastBullet.OwnerId);
        }

        /// <summary>
        /// Проверяет, что ввод команд движения корректно изменяет координаты позиции игрока.
        /// </summary>
        [Fact]
        public void HandleInput_MovementKeys_ChangesPlayerPosition()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[15, 15]);
            var controller = new GameController(map);

            Assert.NotNull(controller.Player1);
            var initialPos = controller.Player1.Position;

            // Act
            controller.HandleInput(GameInput.D);
            controller.HandleInput(GameInput.D);

            // Assert
            Assert.NotEqual(initialPos.X, controller.Player1.Position.X);
        }

        /// <summary>
        /// Проверяет, что команды движения второго игрока (стрелки) также корректно обрабатываются.
        /// </summary>
        [Fact]
        public void HandleInput_Player2MovementKeys_ChangesPlayer2Position()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[15, 15]);
            var controller = new GameController(map);

            Assert.NotNull(controller.Player2);
            var initialPos = controller.Player2.Position;

            // Act
            controller.HandleInput(GameInput.Right);
            controller.HandleInput(GameInput.Right);

            // Assert
            Assert.NotEqual(initialPos.X, controller.Player2.Position.X);
        }
    }
}