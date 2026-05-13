using Xunit;
using Application.Services;
using GameCore.Map;
using System;

namespace GameTests.ServicesTests
{
    /// <summary>
    /// Тестирование алгоритма генерации игрового лабиринта.
    /// Проверяет валидацию входных данных, геометрические свойства и топологию карты.
    /// </summary>
    public class MapGeneratorTests
    {
        private readonly MapGenerator _generator = new MapGenerator();

        /// <summary>
        /// Проверяет, что генератор выбрасывает исключение, если хотя бы один из размеров четный.
        /// Нечетные размеры критичны для корректной расстановки стен и проходов.
        /// </summary>
        [Fact]
        public void GenerateMaze_EvenDimensions_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => _generator.GenerateMaze(10, 11));
            Assert.Throws<ArgumentException>(() => _generator.GenerateMaze(11, 20));
        }

        /// <summary>
        /// Проверяет, что созданная карта имеет ровно те размеры, которые были запрошены.
        /// </summary>
        [Fact]
        public void GenerateMaze_ValidOddDimensions_CreatesMapWithCorrectSize()
        {
            // Arrange
            int expectedWidth = 15;
            int expectedHeight = 15;

            // Act
            var map = _generator.GenerateMaze(expectedWidth, expectedHeight);

            // Assert
            Assert.Equal(expectedWidth, map.Width());
            Assert.Equal(expectedHeight, map.Height());
        }

        /// <summary>
        /// Проверяет наличие центральной симметрии карты.
        /// Это гарантирует, что условия для игроков, стартующих в противоположных углах, будут равными.
        /// </summary>
        [Fact]
        public void GenerateMaze_GeneratedLayout_IsCentrallySymmetric()
        {
            // Arrange
            int w = 21;
            int h = 21;
            var map = _generator.GenerateMaze(w, h);

            // Act & Assert
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool originalIsWall = map.IsWall(x, y);
                    bool mirroredIsWall = map.IsWall(w - 1 - x, h - 1 - y);

                    Assert.True(originalIsWall == mirroredIsWall,
                        $"Нарушена симметрия в точке ({x}, {y}) относительно ({w - 1 - x}, {h - 1 - y})");
                }
            }
        }

        /// <summary>
        /// Проверяет, что центр карты всегда остается свободным от стен.
        /// Обычно это требуется для размещения бонусов или предотвращения тупиков в центре.
        /// </summary>
        [Fact]
        public void GenerateMaze_CenterPoint_AlwaysEmpty()
        {
            // Arrange
            int size = 11;
            int centerIndex = size / 2;

            // Act
            var map = _generator.GenerateMaze(size, size);

            // Assert
            Assert.False(map.IsWall(centerIndex, centerIndex), "Центральная клетка должна быть проходимой.");
        }

        /// <summary>
        /// Проверяет, что внешние границы карты (периметр) полностью состоят из стен.
        /// </summary>
        [Fact]
        public void GenerateMaze_Perimeter_IsSolidWall()
        {
            // Arrange
            int w = 11;
            int h = 11;
            var map = _generator.GenerateMaze(w, h);

            // Act & Assert
            for (int x = 0; x < w; x++)
            {
                Assert.True(map.IsWall(x, 0), $"Верхняя граница пробита в X={x}");
                Assert.True(map.IsWall(x, h - 1), $"Нижняя граница пробита в X={x}");
            }
            for (int y = 0; y < h; y++)
            {
                Assert.True(map.IsWall(0, y), $"Левая граница пробита в Y={y}");
                Assert.True(map.IsWall(w - 1, y), $"Правая граница пробита в Y={y}");
            }
        }
    }
}