using Xunit;
using GameCore;
using System.Drawing;

namespace GameTests.EntitiesTests
{
    /// <summary>
    /// Тестирование базового функционала пространственных игровых объектов.
    /// </summary>
    public class GameObjectTests
    {
        /// <summary>
        /// Тестовая реализация абстрактного класса <see cref="GameObject"/> для проверки базовой логики.
        /// </summary>
        private class MockGameObject : GameObject
        {
            /// <summary>
            /// Инициализирует новый экземпляр тестового игрового объекта.
            /// </summary>
            /// <param name="x">Координата X центра объекта.</param>
            /// <param name="y">Координата Y центра объекта.</param>
            /// <param name="size">Размер коллизии объекта.</param>
            public MockGameObject(float x, float y, float size) : base(x, y, size) { }
        }

        /// <summary>
        /// Проверяет, что метод GetBounds корректно вычисляет границы объекта, 
        /// центрируя прямоугольник относительно позиции объекта.
        /// </summary>
        [Fact]
        public void GetBounds_FixedPositionAndSize_CalculatesCorrectCenteredRectangle()
        {
            // Arrange
            var obj = new MockGameObject(100f, 100f, 50f);

            // Act
            RectangleF bounds = obj.GetBounds();

            // Assert
            Assert.Equal(75f, bounds.X);
            Assert.Equal(75f, bounds.Y);
            Assert.Equal(50f, bounds.Width);
            Assert.Equal(50f, bounds.Height);
        }

        /// <summary>
        /// Проверяет, что состояние активности объекта по умолчанию установлено в <see langword="true"/>.
        /// </summary>
        [Fact]
        public void IsActive_DefaultState_ReturnsTrue()
        {
            // Arrange & Act
            var obj = new MockGameObject(0, 0, 10);

            // Assert
            Assert.True(obj.IsActive);
        }
    }
}