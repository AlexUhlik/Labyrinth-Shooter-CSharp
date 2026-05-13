using System;

namespace GameCore
{
    /// <summary>
    /// Представляет точку или вектор в двумерном пространстве с использованием координат с плавающей запятой.
    /// Используется для определения местоположения объектов и выполнения векторных операций.
    /// </summary>
    public struct Point
    {
        /// <summary> Координата по оси X. </summary>
        public float X { get; set; }

        /// <summary> Координата по оси Y. </summary>
        public float Y { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр структуры Point.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary> Сложение двух векторов. </summary>
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        /// <summary> Увеличение координат вектора на константу. </summary>
        public static Point operator +(Point a, float number)
        {
            return new Point(a.X + number, a.Y + number);
        }

        /// <summary> Вычитание одного вектора из другого. </summary>
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        } 

        /// <summary> Уменьшение координат вектора на константу. </summary>
        public static Point operator -(Point a, float number)
        {
            return new Point(a.X - number, a.Y - number);
        }

        /// <summary> Умножение вектора на число (скаляр). Полезно для расчета скорости. </summary>
        public static Point operator *(Point a, float multiplier)
        {
            return new Point(a.X * multiplier, a.Y * multiplier);
        }
    }
}