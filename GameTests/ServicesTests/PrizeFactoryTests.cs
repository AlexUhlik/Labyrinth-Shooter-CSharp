using Xunit;
using Application.Services;
using GameCore.Items;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Тестирование фабрики призов (PrizeFactory).
    /// Проверяет создание конкретных типов бонусов и логику их симметричного размещения на карте.
    /// </summary>
    public class PrizeFactoryTests
    {
        /// <summary>
        /// Проверяет, что фабрика создает правильный подтип Prize на основе переданного индекса.
        /// </summary>
        /// <param name="typeIndex">Индекс типа бонуса.</param>
        /// <param name="expectedType">Ожидаемый класс реализации.</param>
        [Theory]
        [InlineData(0, typeof(HealthPrize))]
        [InlineData(1, typeof(AmmunitionPrize))]
        [InlineData(2, typeof(ExplosivePrize))]
        [InlineData(3, typeof(FastPrize))]
        public void CreatePrize_IndexInput_ReturnsCorrectSubclass(int typeIndex, Type expectedType)
        {
            // Arrange & Act
            var prize = PrizeFactory.CreatePrize(typeIndex, 0, 0);

            // Assert
            Assert.IsType(expectedType, prize);
        }

        /// <summary>
        /// Проверяет механизм генерации пары бонусов. 
        /// Бонусы должны быть идентичного типа и располагаться симметрично относительно центра карты.
        /// </summary>
        [Fact]
        public void SpawnRandomPair_StandardMap_ReturnsTwoSymmetricPrizesOfSameType()
        {
            // Arrange
            int mapSize = 11;
            var map = new LabyrinthMap(new TileType[mapSize, mapSize]);
            int forcedType = 1; // AmmunitionPrize
            float tileSize = 50f; 
            float worldWidth = mapSize * tileSize;
            float worldHeight = mapSize * tileSize;

            // Act
            var prizes = PrizeFactory.SpawnRandomPair(map, forceType: forcedType);

            // Assert
            Assert.Equal(2, prizes.Count);
            Assert.IsType<AmmunitionPrize>(prizes[0]);
            Assert.IsType<AmmunitionPrize>(prizes[1]);

            Assert.Equal(worldWidth, prizes[0].Position.X + prizes[1].Position.X, precision: 1);
            Assert.Equal(worldHeight, prizes[0].Position.Y + prizes[1].Position.Y, precision: 1);
        }

        /// <summary>
        /// Проверяет, что при передаче некорректного индекса типа фабрика выбрасывает исключение.
        /// </summary>
        [Fact]
        public void CreatePrize_InvalidIndex_ThrowsArgumentException()
        {
            // Arrange
            int invalidType = 999;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PrizeFactory.CreatePrize(invalidType, 0, 0));
        }
    }
}