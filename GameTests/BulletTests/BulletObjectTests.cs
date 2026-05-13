using GameCore.Bullets;
using GameCore;
using Xunit;

namespace GameTests.BulletTests
{
    /// <summary>
    /// Тестирование базовой логики поведения объекта снаряда и работы паттерна "Декоратор".
    /// </summary>
    public class BulletObjectTests
    {
        /// <summary>
        /// Проверяет, что метод Update корректно изменяет координаты снаряда 
        /// на основе его скорости и вектора направления.
        /// </summary>
        [Fact]
        public void Update_MovingDiagonal_ChangesPositionBasedOnSpeed()
        {
            // Arrange
            var stats = new StandartBullet(); 
            // Начало в (0,0), вектор направления (1, 1)
            var bullet = new Bullet(0, 0, 1, 1, stats, ownerId: 1);

            // Act
            bullet.Update();

            // Assert
            Assert.Equal(8.0f, bullet.Position.X);
            Assert.Equal(8.0f, bullet.Position.Y);
        }

        /// <summary>
        /// Проверяет, что снаряд корректно делегирует получение значения урона объекту характеристик.
        /// </summary>
        [Fact]
        public void GetDamage_StandardStats_ReturnsDefaultDamageValue()
        {
            // Arrange
            var stats = new StandartBullet(); 
            var bullet = new Bullet(0, 0, 0, 0, stats, 1);

            // Act & Assert
            Assert.Equal(10, bullet.GetDamage());
        }

        /// <summary>
        /// Проверяет работу метода GetBase, который должен возвращать исходный объект 
        /// снаряда, проходя через любую цепочку декораторов.
        /// </summary>
        [Fact]
        public void GetBase_MultipleDecorators_ReturnsOriginalStandartBullet()
        {
            // Arrange
            var baseBullet = new StandartBullet();
            var decorator1 = new FastAmmo(baseBullet, 10f);
            var decorator2 = new ExplosiveAmmo(decorator1, 10f);

            // Act
            var result = decorator2.GetBase();

            // Assert
            Assert.Same(baseBullet, result);
            Assert.IsType<StandartBullet>(result);
        }
    }
}