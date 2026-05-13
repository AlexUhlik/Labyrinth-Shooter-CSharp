using Xunit;
using GameCore.Items;
using GameCore.Characters;

namespace GameTests.PrizeTests
{
    /// <summary>
    /// Набор тестов для проверки механики восстановления здоровья при поднятии аптечки.
    /// </summary>
    public class HealthPrizeTests
    {
        /// <summary>
        /// Проверяет, что здоровье игрока восстанавливается, но не превышает установленный лимит MaxHealth (100).
        /// </summary>
        [Fact]
        public void ApplyEffect_NearMaxHealth_ClampsToMaxHealth()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            player.Health = 90; 
            var prize = new HealthPrize(0, 0);

            // Act
            prize.ApplyEffect(player);

            // Assert
            Assert.Equal(100, player.Health);
        }

        /// <summary>
        /// Проверяет корректное увеличение здоровья, когда сумма текущего HP и бонуса не превышает максимум.
        /// </summary>
        [Fact]
        public void ApplyEffect_LowHealth_IncreasesHealthByStandardValue()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            player.Health = 40;
            var prize = new HealthPrize(0, 0);

            // Act
            prize.ApplyEffect(player);

            // Assert
            // 40 + 25 = 65
            Assert.Equal(65, player.Health);
        }

        /// <summary>
        /// Проверяет инициализацию позиции через конструктор.
        /// </summary>
        [Fact]
        public void Constructor_CoordinatesInput_SetsPositionCorrectly()
        {
            // Arrange & Act
            float expectedX = 42f;
            float expectedY = 84f;
            var prize = new HealthPrize(expectedX, expectedY);

            // Assert
            Assert.Equal(expectedX, prize.Position.X);
            Assert.Equal(expectedY, prize.Position.Y);
        }
    }
}