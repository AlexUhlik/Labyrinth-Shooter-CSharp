using Xunit;
using Application.Services;
using GameCore.Characters;
using GameCore.Map;
using System.Collections.Generic;
using System.Drawing;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Тестирование фабрики легких врагов (LightEnemyFactory).
    /// Проверяет базовые характеристики юнитов, которые являются "пушечным мясом" в игре.
    /// </summary>
    public class LightEnemyFactoryTests
    {
        /// <summary>
        /// Проверяет, что метод CreateEnemy устанавливает стандартные характеристики:
        /// невысокое здоровье, базовую скорость и характерный зеленый цвет.
        /// </summary>
        [Fact]
        public void CreateEnemy_ValidCoordinates_SetsCorrectLightStatsAndColor()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var players = new List<Player>();
            var factory = new LightEnemyFactory(map, players);
            float spawnX = 100f;
            float spawnY = 150f;

            // Act
            var enemy = factory.CreateEnemy(spawnX, spawnY);

            // Assert
            // Проверка позиции инициализации
            Assert.Equal(spawnX, enemy.Position.X);
            Assert.Equal(spawnY, enemy.Position.Y);

            Assert.Equal(50, enemy.Health);         // Минимальное здоровье
            Assert.Equal(2.0f, enemy.Speed);       // Базовая скорость
            Assert.Equal(100, enemy.Score);        // Минимальная награда

            Assert.Equal(Color.FromArgb(77, 230, 77), enemy.DisplayColor);
        }

        /// <summary>
        /// Проверяет, что фабрика корректно наследует базовый функционал и не возвращает null.
        /// </summary>
        [Fact]
        public void CreateEnemy_Always_ReturnsNotNullEnemy()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var factory = new LightEnemyFactory(map, new List<Player>());

            // Act
            var enemy = factory.CreateEnemy(0, 0);

            // Assert
            Assert.NotNull(enemy);
        }
    }
}