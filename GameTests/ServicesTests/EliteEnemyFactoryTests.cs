using Xunit;
using Application.Services;
using GameCore.Characters;
using GameCore.Map;
using System.Collections.Generic;
using System.Drawing;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Набор тестов для проверки фабрики EliteEnemyFactory.
    /// Проверяет, что создаваемые юниты обладают специфическими характеристиками, соответствующими их рангу.
    /// </summary>
    public class EliteEnemyFactoryTests
    {
        /// <summary>
        /// Проверяет, что метод CreateEnemy устанавливает правильные характеристики здоровья, 
        /// скорости, очков и цвета для элитного противника.
        /// </summary>
        [Fact]
        public void CreateEnemy_ValidCoordinates_SetsCorrectEliteStatsAndColor()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var players = new List<Player>();
            var factory = new EliteEnemyFactory(map, players);
            float spawnX = 50f;
            float spawnY = 75f;

            // Act
            var enemy = factory.CreateEnemy(spawnX, spawnY);

            // Assert

            Assert.Equal(spawnX, enemy.Position.X);
            Assert.Equal(spawnY, enemy.Position.Y);

            Assert.Equal(100, enemy.Health);        // Стандартное здоровье для элиты
            Assert.Equal(4.0f, enemy.Speed);        // Высокая скорость перемещения
            Assert.Equal(300, enemy.Score);         // Награда за уничтожение

            Assert.Equal(Color.FromArgb(51, 179, 255), enemy.DisplayColor);
        }

        /// <summary>
        /// Проверяет, что фабрика создаёт врага, который не является null.
        /// </summary>
        [Fact]
        public void CreateEnemy_Always_ReturnsNotNullEnemy()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var factory = new EliteEnemyFactory(map, new List<Player>());

            // Act
            var enemy = factory.CreateEnemy(0, 0);

            // Assert
            Assert.NotNull(enemy);
        }
    }
}