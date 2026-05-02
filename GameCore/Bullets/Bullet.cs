using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Представляет физический объект снаряда, летящий в игровом пространстве.
    /// </summary>
    public class Bullet : GameObject
    {
        /// <summary> Ссылка на интерфейс характеристик пули. </summary>
        public IBullet Stats { get; private set; }

        /// <summary> Направление движения по оси X. </summary>
        public float DirX { get; }

        /// <summary> Направление движения по оси Y. </summary>
        public float DirY { get; }

        /// <summary> Идентификатор создателя снаряда. </summary>
        public int OwnerId { get; }

        /// <summary>
        /// Создает новый экземпляр пули.
        /// </summary>
        /// <param name="x">Начальная координата X.</param>
        /// <param name="y">Начальная координата Y.</param>
        /// <param name="dirX">Направление X.</param>
        /// <param name="dirY">Направление Y.</param>
        /// <param name="stats">Объект характеристик.</param>
        /// <param name="ownerId">ID объекта, выпустившего пулю.</param>
        public Bullet(float x, float y, float dirX, float dirY, IBullet stats, int ownerId)
            : base(x, y, 12f)
        {
            Stats = stats;
            DirX = dirX;
            DirY = dirY;
            OwnerId = ownerId;
        }

        /// <summary>
        /// Обновляет положение пули в пространстве на основе её скорости и направления.
        /// Вызывается каждый кадр игрового цикла.
        /// </summary>
        public void Update()
        {
            float speed = Stats.GetSpeed();

            // Вычисляем новую позицию
            Position = new Point(Position.X + DirX * speed, Position.Y + DirY * speed);
        }

        /// <summary>
        /// Возвращает урон снаряда, делегируя расчет объекту характеристик.
        /// </summary>
        public int GetDamage() => Stats.GetDamage();
    }
}