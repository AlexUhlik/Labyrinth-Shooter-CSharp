using GameCore.Characters;
using System;

namespace GameCore.Items
{
    /// <summary>
    /// Представляет игровой бонус "Аптечка".
    /// При подборе восстанавливает фиксированное количество очков здоровья игрока.
    /// </summary>
    public class HealthPrize : Prize
    {
        /// <summary>
        /// Создает экземпляр бонуса здоровья в заданных координатах.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public HealthPrize(float x, float y) : base(x, y)
        {
        }

        /// <summary>
        /// Реализует эффект восстановления здоровья.
        /// </summary>
        /// <param name="player">Игрок, на которого будет наложен эффект.</param>
        public override void ApplyEffect(Player player)
        {
            int restoredHealth = player.Health + 25;

            player.Health = Math.Min(restoredHealth, Player.MaxHealth);
        }
    }
}