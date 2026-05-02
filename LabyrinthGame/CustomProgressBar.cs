using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace LabyrinthGame
{
    /// <summary>
    /// Представляет графический элемент управления в виде полосы прогресса.
    /// Используется для визуализации здоровья, брони или других числовых показателей.
    /// </summary>
    public class CustomProgressBar : Control
    {
        /// <summary>
        /// Текущее значение индикатора.
        /// </summary>
        [Category("Appearance")]
        [Description("Текущее количественное значение полосы прогресса.")]
        public int Value { get; set; } = 100;

        /// <summary>
        /// Максимально возможное значение индикатора.
        /// </summary>
        [Category("Appearance")]
        [Description("Максимальное значение, принимаемое за 100% заполнения.")]
        public int MaxValue { get; set; } = 100;

        /// <summary>
        /// Цвет заполняющей части индикатора.
        /// </summary>
        [Category("Appearance")]
        [Description("Цвет основной полосы индикатора.")]
        public Color BarColor { get; set; } = Color.Red;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomProgressBar"/>.
        /// </summary>
        public CustomProgressBar()
        {
            // Включение двойной буферизации для минимизации мерцания при перерисовке
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Выполняет отрисовку элемента управления.
        /// </summary>
        /// <param name="e">Объект <see cref="PaintEventArgs"/>, содержащий данные события отрисовки.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Отрисовка фонового прямоугольника (пустая часть шкалы)
            e.Graphics.FillRectangle(Brushes.DimGray, 0, 0, this.Width, this.Height);

            if (MaxValue <= 0) return;

            // Расчет ширины заполнения на основе текущего значения
            float percent = (float)Value / MaxValue;
            float fillWidth = percent * this.Width;

            // Отрисовка активной части шкалы выбранным цветом
            using (Brush brush = new SolidBrush(BarColor))
            {
                e.Graphics.FillRectangle(brush, 0, 0, fillWidth, this.Height);
            }

            // Отрисовка контурной рамки
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}