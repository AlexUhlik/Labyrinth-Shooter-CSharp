using Xunit;
using GameCore.Items;
using GameCore.Characters;
using GameCore.Bullets;

namespace GameTests.PrizeTests
{
    /// <summary>
    /// Тестирование логики работы игрового бонуса "Скорострельные боеприпасы".
    /// </summary>
    public class FastPrizeTests
    {
        /// <summary>
        /// Проверяет, что при подборе бонуса текущий снаряд игрока оборачивается в декоратор <see cref="FastAmmo"/>.
        /// </summary>
        [Fact]
        public void ApplyEffect_PlayerEntity_WrapsCurrentBulletInFastDecorator()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            var prize = new FastPrize(0, 0);
            var previousBullet = player.CurrentBullet;

            // Act
            prize.ApplyEffect(player);

            // Assert
            Assert.IsType<FastAmmo>(player.CurrentBullet);

            if (player.CurrentBullet is BulletDecorator decorator)
            {
                Assert.Same(previousBullet, decorator.Inner);
            }
            else
            {
                Assert.Fail("CurrentBullet должен быть типа BulletDecorator после применения приза.");
            }
        }

        /// <summary>
        /// Проверяет корректность инициализации позиции бонуса через конструктор.
        /// </summary>
        [Fact]
        public void Constructor_CoordinatesInput_SetsPositionCorrectly()
        {
            // Arrange & Act
            float expectedX = 500f;
            float expectedY = 100f;
            var prize = new FastPrize(expectedX, expectedY);

            // Assert
            Assert.Equal(expectedX, prize.Position.X);
            Assert.Equal(expectedY, prize.Position.Y);
        }
    }
}