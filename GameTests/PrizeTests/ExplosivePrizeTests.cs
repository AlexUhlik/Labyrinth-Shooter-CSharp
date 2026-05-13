using Xunit;
using GameCore.Items;
using GameCore.Characters;
using GameCore.Bullets;

namespace GameTests.PrizeTests
{
    /// <summary>
    /// Тестирование логики работы игрового бонуса "Взрывные пули".
    /// </summary>
    public class ExplosivePrizeTests
    {
        /// <summary>
        /// Проверяет, что при подборе бонуса текущий снаряд игрока оборачивается в декоратор <see cref="ExplosiveAmmo"/>.
        /// </summary>
        [Fact]
        public void ApplyEffect_PlayerEntity_WrapsCurrentBulletInExplosiveDecorator()
        {
            // Arrange
            var player = new Player(1, 0, 0);
            var prize = new ExplosivePrize(0, 0);
            var previousBullet = player.CurrentBullet;

            // Act
            prize.ApplyEffect(player);

            // Assert
            Assert.IsType<ExplosiveAmmo>(player.CurrentBullet);

            if (player.CurrentBullet is BulletDecorator decorator)
            {
                Assert.Same(previousBullet, decorator.Inner);
            }
            else
            {
                Assert.Fail("CurrentBullet should be a BulletDecorator after applying the prize.");
            }
        }

        /// <summary>
        /// Проверяет, что конструктор корректно устанавливает начальные координаты объекта приза.
        /// </summary>
        [Fact]
        public void Constructor_CoordinatesInput_SetsPositionCorrectly()
        {
            // Arrange & Act
            float expectedX = 100f;
            float expectedY = 200f;
            var prize = new ExplosivePrize(expectedX, expectedY);

            // Assert
            Assert.Equal(expectedX, prize.Position.X);
            Assert.Equal(expectedY, prize.Position.Y);
        }
    }
}