using GameCore.Characters;
using System;

namespace GameCore.Items
{
    /// <summary>
    /// Представляет игровой бонус "Боеприпасы".
    /// При подборе пополняет текущий запас патронов игрока.
    /// </summary>
    public class AmmunitionPrize : Prize
    {
        /// <summary>
        /// Создает экземпляр бонуса боеприпасов в заданных координатах.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public AmmunitionPrize(float x, float y) : base(x, y)
        {
        }

        /// <summary>
        /// Реализует эффект пополнения боезапаса.
        /// Увеличивает количественный показатель Ammunition у объекта игрока.
        /// </summary>
        /// <param name="player">Игрок, подобравший бонус.</param>
        public override void ApplyEffect(Player player)
        {
            player.Ammunition += 25;
        }
    }
}