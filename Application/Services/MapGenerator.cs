using GameCore.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MapGenerator
    {
        private Random random = new Random();

        /// <summary>
        /// Генерирует случайный лабиринт заданного размера.
        /// </summary>
        /// <param name="width">Ширина лабиринта (должна быть нечетной).</param>
        /// <param name="height">Высота лабиринта (должна быть нечетной).</param>
        /// <returns>Двумерный массив TileType, представляющий лабиринт.</returns>
        public TileType[,] GenerateMaze(int width, int height)
        {
            // Алгоритм лучше всего работает с нечетными размерами
            if (width % 2 == 0 || height % 2 == 0)
            {
                throw new ArgumentException("Ширина и высота лабиринта должны быть нечетными.");
            }

            // 1. Создаем сетку, полностью заполненную стенами
            var maze = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    maze[x, y] = TileType.Wall;
                }
            }

            // Стек для отслеживания пути "копателя" (для возврата из тупиков)
            var path = new Stack<(int x, int y)>();

            // 2. Выбираем случайную стартовую точку (обязательно нечетные координаты)
            int startX = random.Next(1, width / 2) * 2 - 1;
            int startY = random.Next(1, height / 2) * 2 - 1;

            maze[startX, startY] = TileType.Empty; // "Расчищаем" стартовую клетку
            path.Push((startX, startY)); // Добавляем ее в путь

            while (path.Count > 0)
            {
                var (currentX, currentY) = path.Peek(); // Смотрим, где мы сейчас

                // 3. Находим всех "непосещенных" соседей
                var neighbors = new List<(int x, int y)>();

                // Проверяем соседа сверху
                if (currentY - 2 >= 0 && maze[currentX, currentY - 2] == TileType.Wall)
                    neighbors.Add((currentX, currentY - 2));
                // Снизу
                if (currentY + 2 < height && maze[currentX, currentY + 2] == TileType.Wall)
                    neighbors.Add((currentX, currentY + 2));
                // Слева
                if (currentX - 2 >= 0 && maze[currentX - 2, currentY] == TileType.Wall)
                    neighbors.Add((currentX - 2, currentY));
                // Справа
                if (currentX + 2 < width && maze[currentX + 2, currentY] == TileType.Wall)
                    neighbors.Add((currentX + 2, currentY));

                if (neighbors.Count > 0)
                {
                    // 4. Если есть соседи, случайно выбираем одного
                    var (nextX, nextY) = neighbors[random.Next(neighbors.Count)];

                    // 5. "Пробиваем" стену между текущей и следующей клеткой
                    int wallX = currentX + (nextX - currentX) / 2;
                    int wallY = currentY + (nextY - currentY) / 2;
                    maze[wallX, wallY] = TileType.Empty;

                    // 6. Переходим в новую клетку
                    maze[nextX, nextY] = TileType.Empty;
                    path.Push((nextX, nextY));
                }
                else
                {
                    // 7. Если соседей нет (тупик), возвращаемся назад
                    path.Pop();
                }
            }

            return maze;
        }
    }
}
