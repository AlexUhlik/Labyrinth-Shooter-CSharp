using Xunit;
using Application.Services;
using GameCore.Characters;
using GameCore.Map;
using System.Collections.Generic;
using System;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Тестирование базовой логики фабрик врагов, общей для всех типов противников.
    /// Основное внимание уделяется алгоритмам размещения юнитов на карте.
    /// </summary>
    public class EnemyFactoryBaseTests
    {
        /// <summary>
        /// Проверяет, что метод SpawnRandom генерирует позицию для врага, 
        /// которая находится не ближе заданного расстояния (safeDistance) от игрока.
        /// </summary>
        [Fact]
        public void SpawnRandom_WithPlayerPresent_RespectsSafeDistanceConstraint()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[10, 10]);

            var player = new Player(1, 0, 0);
            var players = new List<Player> { player };

            var factory = new LightEnemyFactory(map, players);
            float safeDistance = 200f;

            // Act
            var enemy = factory.SpawnRandom(safeDistance);

            // Assert
            Assert.NotNull(enemy);

            float dx = enemy.Position.X - player.Position.X;
            float dy = enemy.Position.Y - player.Position.Y;
            float actualDistance = (float)Math.Sqrt(dx * dx + dy * dy);

            Assert.True(actualDistance >= safeDistance,
                $"Враг заспавнился слишком близко к игроку. Дистанция: {actualDistance}, ожидалось минимум: {safeDistance}");
        }

        /// <summary>
        /// Проверяет, что при отсутствии игроков на карте метод SpawnRandom 
        /// все равно успешно создает врага в любой доступной точке.
        /// </summary>
        [Fact]
        public void SpawnRandom_NoPlayersOnMap_StillReturnsEnemy()
        {
            // Arrange
            var map = new LabyrinthMap(new TileType[5, 5]);
            var factory = new LightEnemyFactory(map, new List<Player>());

            // Act
            var enemy = factory.SpawnRandom(0f);

            // Assert
            Assert.NotNull(enemy);
        }
    }
}