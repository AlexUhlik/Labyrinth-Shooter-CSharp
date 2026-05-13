using Xunit;
using GameCore.Bullets;

namespace GameTests.BulletTests
{
    /// <summary>
    /// Тестирование декоратора "Взрывные боеприпасы", увеличивающего урон снаряда.
    /// </summary>
    public class ExplosiveAmmoTests
    {
        /// <summary>
        /// Проверяет, что взрывные боеприпасы корректно добавляют бонусный урон к базовому значению снаряда.
        /// </summary>
        [Fact]
        public void GetDamage_ExplosiveModifier_AddsBonusToBaseDamage()
        {
            // Arrange
            var baseBullet = new StandartBullet(); // Базовый урон: 10
            var explosive = new ExplosiveAmmo(baseBullet, 10f);

            // Act
            int totalDamage = explosive.GetDamage();

            // Assert
            // Ожидаемый расчет: 10 + 15 (бонус взрыва) = 25
            Assert.Equal(25, totalDamage);
        }

        /// <summary>
        /// Проверяет логику истечения времени действия модификатора.
        /// </summary>
        [Fact]
        public void IsExpired_TimeElapsed_ReturnsTrue()
        {
            // Arrange
            var explosive = new ExplosiveAmmo(new StandartBullet(), 5f);

            // Act
            // Симуляция прохождения 6 секунд при длительности эффекта 5 секунд
            explosive.UpdateTime(6f);

            // Assert
            Assert.True(explosive.IsExpired);
        }

        /// <summary>
        /// Проверяет, что модификатор остается активным, если время действия еще не истекло.
        /// </summary>
        [Fact]
        public void IsExpired_TimeRemaining_ReturnsFalse()
        {
            // Arrange
            var explosive = new ExplosiveAmmo(new StandartBullet(), 5f);

            // Act
            explosive.UpdateTime(2f);

            // Assert
            Assert.False(explosive.IsExpired);
        }
    }
}