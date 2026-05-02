using System;
using System.Drawing;

namespace GameCore
{
    /// <summary>
    /// Абстрактный базовый класс для всех объектов игрового мира.
    /// Определяет фундаментальные свойства: позицию, размер и состояние активности.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// Текущая позиция объекта в мировых координатах.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Габариты объекта.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Указывает, активен ли объект. Неактивные объекты исключаются из обработки и отрисовки.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Инициализирует новый игровой объект.
        /// </summary>
        /// <param name="x">Начальная координата X.</param>
        /// <param name="y">Начальная координата Y.</param>
        /// <param name="size">Размер объекта.</param>
        public GameObject(float x, float y, float size)
        {
            Position = new Point(x, y);
            Size = size;
        }

        /// <summary>
        /// Возвращает ограничивающий прямоугольник объекта для расчета столкновений.
        /// Прямоугольник центрируется относительно текущей позиции объекта.
        /// </summary>
        /// <returns>Экземпляр RectangleF, представляющий границы объекта.</returns>
        public RectangleF GetBounds()
        {
            float offset = Size / 2f;
            return new RectangleF(Position.X - offset, Position.Y - offset, Size, Size);
        }
    }
}