using Xunit;
using GameCore.Bullets;
using System;

namespace GameTests.BulletTests
{
    /// <summary>
    /// Тестирование вспомогательных методов для управления цепочками декораторов снарядов.
    /// </summary>
    public class BulletToolsTests
    {
        /// <summary>
        /// Проверяет, что метод IsDecoratorActive способен найти декоратор определенного типа на любом уровне вложенности.
        /// </summary>
        [Fact]
        public void IsDecoratorActive_NestedDecorators_ReturnsTrueForExistingTypes()
        {
            // Arrange
            IBullet bullet = new StandartBullet();
            bullet = new FastAmmo(bullet, 10f);
            bullet = new ExplosiveAmmo(bullet, 10f);

            // Act & Assert
            Assert.True(BulletTools.IsDecoratorActive<FastAmmo>(bullet));
            Assert.True(BulletTools.IsDecoratorActive<ExplosiveAmmo>(bullet));
            Assert.True(BulletTools.IsDecoratorActive<StandartBullet>(bullet));
        }

        /// <summary>
        /// Проверяет, что метод Cleanup корректно удаляет декораторы с истекшим временем действия из цепочки,
        /// сохраняя при этом активные декораторы.
        /// </summary>
        [Fact]
        public void Cleanup_ExpiredDecoratorInMiddle_RemovesOnlyExpiredAndReconnectsChain()
        {
            // Arrange
            IBullet root = new StandartBullet();
            var expired = new FastAmmo(root, 0f);
            var active = new ExplosiveAmmo(expired, 10f);

            // Act
            var result = BulletTools.Cleanup(active);

            // Assert
            Assert.True(BulletTools.IsDecoratorActive<ExplosiveAmmo>(result));
            Assert.False(BulletTools.IsDecoratorActive<FastAmmo>(result));

            if (result is BulletDecorator decorator)
            {
                Assert.IsType<StandartBullet>(decorator.Inner);
            }
            else
            {
                Assert.Fail("Resulting bullet should still be a decorator.");
            }
        }

        /// <summary>
        /// Проверяет, что если у всех декораторов в цепочке истекло время действия, метод Cleanup возвращает базовый объект снаряда.
        /// </summary>
        [Fact]
        public void Cleanup_AllDecoratorsExpired_ReturnsBaseBullet()
        {
            // Arrange
            IBullet root = new StandartBullet();
            root = new FastAmmo(root, -1f);
            root = new ExplosiveAmmo(root, 0f);

            // Act
            var result = BulletTools.Cleanup(root);

            // Assert
            Assert.IsType<StandartBullet>(result);
            Assert.False(result is BulletDecorator);
        }

        /// <summary>
        /// Проверяет, что метод IsDecoratorActive возвращает false, если искомый тип декоратора отсутствует в цепочке.
        /// </summary>
        [Fact]
        public void IsDecoratorActive_TypeMissingInChain_ReturnsFalse()
        {
            // Arrange
            IBullet bullet = new FastAmmo(new StandartBullet(), 10f);

            // Act
            bool isActive = BulletTools.IsDecoratorActive<ExplosiveAmmo>(bullet);

            // Assert
            Assert.False(isActive);
        }
    }
}