using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Конкретный декоратор, улучшающий скоростные характеристики снаряда.
    /// Увеличивает скорость полета пули и уменьшает задержку между выстрелами.
    /// </summary>
    public class FastAmmo : BulletDecorator
    {
        /// <summary>
        /// Создает экземпляр улучшения "Быстрые патроны".
        /// </summary>
        /// <param name="bullet">Обертываемый объект снаряда.</param>
        /// <param name="duration">Длительность действия эффекта в секундах.</param>
        public FastAmmo(IBullet bullet, float duration) : base(bullet, duration)
        {
        }

        /// <summary>
        /// Переопределяет скорость, увеличивая базовое значение на 50%.
        /// </summary>
        /// <returns>Модифицированная скорость полета.</returns>
        public override float GetSpeed()
        {
            float baseSpeed = base.GetSpeed();
            return baseSpeed * 1.5f;
        }

        /// <summary>
        /// Переопределяет коэффициент задержки, сокращая время перезарядки вдвое.
        /// </summary>
        /// <returns>Модифицированный коэффициент задержки.</returns>
        public override float GetCooldown()
        {
            float baseCooldown = base.GetCooldown();
            return baseCooldown / 2.0f;
        }
    }
}