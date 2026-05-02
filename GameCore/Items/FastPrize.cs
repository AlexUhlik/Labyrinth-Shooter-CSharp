using GameCore.Bullets;
using GameCore.Characters;
using System;

namespace GameCore.Items
{
    /// <summary>
    /// Представляет игровой бонус "Скорострельность".
    /// При подборе оборачивает текущий тип снаряда игрока в декоратор FastAmmo.
    /// </summary>
    public class FastPrize : Prize
    {
        /// <summary>
        /// Создает экземпляр бонуса ускорения в заданных координатах.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public FastPrize(float x, float y) : base(x, y)
        {
        }

        /// <summary>
        /// Реализует эффект улучшения боеприпасов.
        /// Использует паттерн Декоратор для добавления свойств к текущему снаряду.
        /// </summary>
        /// <param name="player">Игрок, подобравший бонус.</param>
        public override void ApplyEffect(Player player)
        {
            player.CurrentBullet = new FastAmmo(player.CurrentBullet, 10f);
        }
    }
}