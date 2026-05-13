using GameCore.Items;
using GameCore.Characters;
using Xunit;

namespace GameTests.PrizeTests
{
    /// <summary>
    /// Тестирование логики работы игрового бонуса "Боеприпасы".
    /// </summary>
    public class AmmunitionPrizeTests
    {
        /// <summary>
        /// Проверяет, что применение эффекта бонуса увеличивает текущее количество патронов игрока на 25 единиц.
        /// </summary>
        [Fact]
        public void ApplyEffect_PlayerEntity_IncreasesAmmunitionByFixedAmount()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            var prize = new AmmunitionPrize(0, 0);
            int initialAmmo = player.Ammunition;

            // Act
            prize.ApplyEffect(player);

            // Assert
            Assert.Equal(initialAmmo + 25, player.Ammunition);
        }

        /// <summary>
        /// Проверяет, что конструктор корректно инициализирует координаты появления бонуса в игровом мире.
        /// </summary>
        [Fact]
        public void Constructor_CoordinatesInput_SetsCorrectInitialPosition()
        {
            // Arrange & Act
            float expectedX = 150f;
            float expectedY = 300f;
            var prize = new AmmunitionPrize(expectedX, expectedY);

            // Assert
            Assert.Equal(expectedX, prize.Position.X);
            Assert.Equal(expectedY, prize.Position.Y);
        }

        /// <summary>
        /// Проверяет, что после создания бонус является активным объектом.
        /// </summary>
        [Fact]
        public void IsActive_DefaultState_ReturnsTrue()
        {
            // Arrange & Act
            var prize = new AmmunitionPrize(0, 0);

            // Assert
            Assert.True(prize.IsActive);
        }
    }
}