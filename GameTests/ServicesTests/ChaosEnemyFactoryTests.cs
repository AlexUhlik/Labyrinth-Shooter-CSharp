using Xunit;
using Application.Services;
using GameCore.Characters;
using GameCore.Map;
using System.Collections.Generic;
using System.Drawing;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Тестирование фабрики ChaosEnemyFactory.
    /// Проверяет специфические характеристики юнитов, создаваемых данной фабрикой.
    /// </summary>
    public class ChaosEnemyFactoryTests
    {
        /// <summary>
        /// Проверяет, что метод CreateEnemy устанавливает уникальные статы для данного типа врагов:
        /// повышенное здоровье, высокую скорость и специфический цвет.
        /// </summary>
        [Fact]
        public void CreateEnemy_ValidCoordinates_SetsCorrectChaosStatsAndColor()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var players = new List<Player>();
            var factory = new ChaosEnemyFactory(map, players);
            float spawnX = 10f;
            float spawnY = 20f;

            // Act
            var enemy = factory.CreateEnemy(spawnX, spawnY);

            // Assert
            Assert.Equal(spawnX, enemy.Position.X);
            Assert.Equal(spawnY, enemy.Position.Y);
            Assert.Equal(250, enemy.Health);        // Здоровье выше стандартного
            Assert.Equal(3.0f, enemy.Speed);        // Высокая скорость передвижения
            Assert.Equal(500, enemy.Score);         // Награда за уничтожение

            Assert.Equal(Color.FromArgb(255, 89, 0), enemy.DisplayColor);
        }

        /// <summary>
        /// Проверяет, что фабрика корректно инициализируется списком игроков для последующей передачи в логику ИИ.
        /// </summary>
        [Fact]
        public void Constructor_ValidArguments_InitializesCorrectly()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var players = new List<Player> { new Player(1, 0, 0) };

            // Act
            var factory = new ChaosEnemyFactory(map, players);

            // Assert
            Assert.NotNull(factory);
        }
    }
}