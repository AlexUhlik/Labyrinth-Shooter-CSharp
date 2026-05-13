using Xunit;
using Application.Game;
using Application.Services;
using GameCore.Map;
using System.Reflection;
using System;
using System.Linq;

namespace GameTests.GameControllerTests
{
    /// <summary>
    /// Тестирование прогрессии сложности игры и механизмов смены фабрик противников.
    /// </summary>
    public class GameControllerProgressionTests
    {
        /// <summary>
        /// Проверяет, что после уничтожения определенного количества врагов (стадия), 
        /// контроллер переключается с базовой фабрики на элитную.
        /// </summary>
        [Fact]
        public void UpdateWorld_KillCountThresholdReached_TransitionsToEliteFactory()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[15, 15]);
            var controller = new GameController(map);

            // Имитируем убийство 10 врагов (EnemiesPerStage), чтобы спровоцировать переход на новый этап.
            for (int i = 0; i < 10; i++)
            {
                var enemy = controller.Enemies.FirstOrDefault();
                if (enemy == null) break;

                enemy.TakeDamage(1000, 1);    // Наносим урон
                controller.UpdateWorld(0.1f); // Запускаем цикл обновления для спавна замены
            }

            FieldInfo? factoryField = typeof(GameController).GetField("_currentEnemyFactory",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (factoryField == null)
            {
                throw new InvalidOperationException("Поле '_currentEnemyFactory' не найдено в GameController.");
            }

            var currentFactory = factoryField.GetValue(controller);

            // Assert
            Assert.NotNull(currentFactory);
            Assert.IsType<EliteEnemyFactory>(currentFactory);
        }

        /// <summary>
        /// Проверяет, что начальная фабрика при создании контроллера является фабрикой легких врагов.
        /// </summary>
        [Fact]
        public void Constructor_InitialState_SetsLightEnemyFactory()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[10, 10]);
            var controller = new GameController(map);

            // Act
            FieldInfo? factoryField = typeof(GameController).GetField("_currentEnemyFactory",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var currentFactory = factoryField?.GetValue(controller);

            // Assert
            Assert.IsType<LightEnemyFactory>(currentFactory);
        }
    }
}