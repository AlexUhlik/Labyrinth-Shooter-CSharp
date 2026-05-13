using Xunit;
using GameCore.Bullets;

namespace GameTests.BulletTests
{
    /// <summary>
    /// Тестирование декоратора "Скоростные боеприпасы", влияющего на скорость полета и задержку стрельбы.
    /// </summary>
    public class FastAmmoTests
    {
        /// <summary>
        /// Проверяет, что модификатор корректно изменяет базовые характеристики скорости и перезарядки.
        /// </summary>
        [Fact]
        public void GetStats_FastAmmoModifier_ModifiesSpeedAndCooldownCorrectly()
        {
            // Arrange
            var baseBullet = new StandartBullet(); // Базовая скорость 8.0f, Задержка 1.0f
            var fast = new FastAmmo(baseBullet, 10f);

            // Act
            float modifiedSpeed = fast.GetSpeed();
            float modifiedCooldown = fast.GetCooldown();

            // Assert
            // Проверка скорости: 8.0 * 1.5 = 12.0
            Assert.Equal(12.0f, modifiedSpeed);

            // Проверка задержки: 1.0 / 2 = 0.5
            Assert.Equal(0.5f, modifiedCooldown);
        }

        /// <summary>
        /// Проверяет, что при наложении декоратора урон снаряда остается неизменным.
        /// </summary>
        [Fact]
        public void GetDamage_FastAmmoModifier_DoesNotChangeBaseDamage()
        {
            // Arrange
            var baseBullet = new StandartBullet(); // Базовый урон 10
            var fast = new FastAmmo(baseBullet, 10f);

            // Act & Assert
            Assert.Equal(10, fast.GetDamage());
        }

        /// <summary>
        /// Проверяет логику истечения времени действия скоростных боеприпасов.
        /// </summary>
        [Fact]
        public void IsExpired_TimeElapsed_ReturnsTrue()
        {
            // Arrange
            var fast = new FastAmmo(new StandartBullet(), 3f);

            // Act
            fast.UpdateTime(4f);

            // Assert
            Assert.True(fast.IsExpired);
        }
    }
}