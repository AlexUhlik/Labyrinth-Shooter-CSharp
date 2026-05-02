using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Конкретный декоратор, увеличивающий поражающую способность снаряда.
    /// Моделирует эффект взрывных боеприпасов путем значительного увеличения урона.
    /// </summary>
    public class ExplosiveAmmo : BulletDecorator
    {
        /// <summary>
        /// Создает экземпляр улучшения "Взрывные патроны".
        /// </summary>
        /// <param name="bullet">Обертываемый объект снаряда.</param>
        /// <param name="duration">Длительность действия эффекта в секундах.</param>
        public ExplosiveAmmo(IBullet bullet, float duration) : base(bullet, duration)
        {
        }

        /// <summary>
        /// Переопределяет урон, добавляя к базовому значению фиксированный бонус.
        /// </summary>
        /// <returns>Суммарный урон (базовый + модификатор).</returns>
        public override int GetDamage()
        {
            int baseDamage = base.GetDamage();

            return baseDamage + 15;
        }
    }
}