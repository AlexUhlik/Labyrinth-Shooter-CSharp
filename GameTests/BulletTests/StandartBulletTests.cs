using Xunit;
using GameCore.Bullets;

namespace GameTests.BulletTests
{
    /// <summary>
    /// Тестирование базового типа снаряда, который служит основой для всех модификаторов.
    /// </summary>
    public class StandartBulletTests
    {
        /// <summary>
        /// Проверяет, что базовый снаряд возвращает корректные значения характеристик по умолчанию.
        /// </summary>
        [Fact]
        public void GetStats_DefaultInstance_ReturnsInitialHardcodedValues()
        {
            // Arrange
            var bullet = new StandartBullet();

            // Act & Assert
            // Проверка базового урона 
            Assert.Equal(10, bullet.GetDamage());

            // Проверка базовой скорости полета 
            Assert.Equal(8.0f, bullet.GetSpeed());

            // Проверка стандартного коэффициента задержки
            Assert.Equal(1.0f, bullet.GetCooldown());

            // Проверка реализации GetBase: базовый объект должен возвращать самого себя
            Assert.Same(bullet, bullet.GetBase());
        }

        /// <summary>
        /// Проверяет, что базовый тип снаряда корректно идентифицируется как StandartBullet.
        /// </summary>
        [Fact]
        public void GetBase_DefaultInstance_IsInstanceOfStandartBullet()
        {
            // Arrange
            var bullet = new StandartBullet();

            // Act
            var baseObject = bullet.GetBase();

            // Assert
            Assert.IsType<StandartBullet>(baseObject);
        }
    }
}