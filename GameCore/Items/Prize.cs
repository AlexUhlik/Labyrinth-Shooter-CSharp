using GameCore.Characters;
using System;

namespace GameCore.Items
{
    /// <summary>
    /// Абстрактный базовый класс для всех подбираемых игровых бонусов.
    /// Определяет логику времени жизни объекта и интерфейс применения эффекта к игроку.
    /// </summary>
    public abstract class Prize : GameObject
    {
        /// <summary> Текущее время существования объекта в секундах. </summary>
        public float Age { get; private set; } = 0;

        /// <summary> Максимальное время жизни объекта. </summary>
        public float MaxLifetime { get; set; } = 25f;

        /// <summary> Возвращает истину, если время жизни объекта истекло. </summary>
        public bool IsExpired => Age >= MaxLifetime;

        /// <summary>
        /// Инициализирует новый экземпляр приза в указанных координатах.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public Prize(float x, float y) : base(x, y, 35)
        {
        }

        /// <summary>
        /// Обновляет состояние приза, увеличивая его возраст.
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с предыдущего кадра.</param>
        public void Update(float deltaTime)
        {
            Age += deltaTime;
        }

        /// <summary>
        /// Абстрактный метод для реализации конкретного эффекта бонуса (здоровье, патроны и т.д.).
        /// Вызывается при пересеччении игрока с объектом приза.
        /// </summary>
        /// <param name="player">Объект игрока, подобравшего приз.</param>
        public abstract void ApplyEffect(Player player);
    }
}