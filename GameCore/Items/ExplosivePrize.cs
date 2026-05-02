using GameCore.Bullets;
using GameCore.Characters;
using System;

namespace GameCore.Items
{
    /// <summary>
    /// Представляет игровой бонус "Взрывные снаряды".
    /// При подборе наделяет текущие пули игрока способностью наносить больший урон.
    /// </summary>
    public class ExplosivePrize : Prize
    {
        /// <summary>
        /// Создает экземпляр взрывного бонуса в заданных координатах.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public ExplosivePrize(float x, float y) : base(x, y)
        {
        }

        /// <summary>
        /// Реализует эффект модификации вооружения..
        /// </summary>
        /// <param name="player">Игрок, подобравший бонус.</param>
        public override void ApplyEffect(Player player)
        {
            player.CurrentBullet = new ExplosiveAmmo(player.CurrentBullet, 15f);
        }
    }
}