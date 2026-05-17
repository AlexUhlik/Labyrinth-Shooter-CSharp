using Xunit;
using Application.Services.PrizeFactory;
using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Тестирование фабричного метода для призов (PrizeSpawner).
    /// Проверяет создание конкретных типов бонусов через конкретные фабрики
    /// и логику их симметричного размещения на карте.
    /// </summary>
    public class PrizeFactoryTests
    {
        /// <summary>
        /// Проверяет, что каждая конкретная фабрика создаёт правильный подтип Prize.
        /// </summary>
        [Fact]
        public void HealthPrizeSpawner_CreatePrize_ReturnsHealthPrize()
        {
            // Arrange
            var spawner = new HealthPrizeSpawner();

            // Act
            var prize = spawner.CreatePrize(0, 0);

            // Assert
            Assert.IsType<HealthPrize>(prize);
        }

        [Fact]
        public void AmmunitionPrizeSpawner_CreatePrize_ReturnsAmmunitionPrize()
        {
            // Arrange
            var spawner = new AmmunitionPrizeSpawner();

            // Act
            var prize = spawner.CreatePrize(0, 0);

            // Assert
            Assert.IsType<AmmunitionPrize>(prize);
        }

        [Fact]
        public void ExplosivePrizeSpawner_CreatePrize_ReturnsExplosivePrize()
        {
            // Arrange
            var spawner = new ExplosivePrizeSpawner();

            // Act
            var prize = spawner.CreatePrize(0, 0);

            // Assert
            Assert.IsType<ExplosivePrize>(prize);
        }

        [Fact]
        public void FastPrizeSpawner_CreatePrize_ReturnsFastPrize()
        {
            // Arrange
            var spawner = new FastPrizeSpawner();

            // Act
            var prize = spawner.CreatePrize(0, 0);

            // Assert
            Assert.IsType<FastPrize>(prize);
        }

        /// <summary>
        /// Проверяет, что шаблонный метод SpawnPair создаёт пару бонусов одного типа.
        /// Бонусы должны располагаться симметрично относительно центра карты.
        /// </summary>
        [Fact]
        public void SpawnPair_WithHealthSpawner_ReturnsTwoSymmetricHealthPrizes()
        {
            // Arrange
            int mapSize = 11;
            var grid = new TileType[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
                for (int j = 0; j < mapSize; j++)
                    grid[i, j] = TileType.Empty; // Все клетки свободны для теста

            var map = new LabyrinthMap(grid);
            var spawner = new HealthPrizeSpawner();
            float tileSize = 50f;
            float worldWidth = mapSize * tileSize;
            float worldHeight = mapSize * tileSize;

            // Act
            var prizes = spawner.SpawnPair(map);

            // Assert
            Assert.Equal(2, prizes.Count);
            Assert.IsType<HealthPrize>(prizes[0]);
            Assert.IsType<HealthPrize>(prizes[1]);

            // Проверка симметричности
            Assert.Equal(worldWidth, prizes[0].Position.X + prizes[1].Position.X, precision: 1);
            Assert.Equal(worldHeight, prizes[0].Position.Y + prizes[1].Position.Y, precision: 1);
        }

        [Fact]
        public void SpawnPair_WithAmmunitionSpawner_ReturnsTwoSymmetricAmmunitionPrizes()
        {
            // Arrange
            int mapSize = 11;
            var grid = new TileType[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
                for (int j = 0; j < mapSize; j++)
                    grid[i, j] = TileType.Empty;

            var map = new LabyrinthMap(grid);
            var spawner = new AmmunitionPrizeSpawner();
            float tileSize = 50f;
            float worldWidth = mapSize * tileSize;
            float worldHeight = mapSize * tileSize;

            // Act
            var prizes = spawner.SpawnPair(map);

            // Assert
            Assert.Equal(2, prizes.Count);
            Assert.IsType<AmmunitionPrize>(prizes[0]);
            Assert.IsType<AmmunitionPrize>(prizes[1]);

            Assert.Equal(worldWidth, prizes[0].Position.X + prizes[1].Position.X, precision: 1);
            Assert.Equal(worldHeight, prizes[0].Position.Y + prizes[1].Position.Y, precision: 1);
        }

        /// <summary>
        /// Проверяет, что разные фабрики создают призы разных типов.
        /// </summary>
        [Fact]
        public void DifferentSpawners_CreateDifferentPrizeTypes()
        {
            // Arrange
            PrizeSpawner[] spawners = new PrizeSpawner[]
            {
                new HealthPrizeSpawner(),
                new AmmunitionPrizeSpawner(),
                new ExplosivePrizeSpawner(),
                new FastPrizeSpawner()
            };

            // Act & Assert
            Assert.IsType<HealthPrize>(spawners[0].CreatePrize(0, 0));
            Assert.IsType<AmmunitionPrize>(spawners[1].CreatePrize(0, 0));
            Assert.IsType<ExplosivePrize>(spawners[2].CreatePrize(0, 0));
            Assert.IsType<FastPrize>(spawners[3].CreatePrize(0, 0));
        }
    }
}