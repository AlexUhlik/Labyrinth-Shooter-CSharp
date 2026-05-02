using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Реализация стандартного типа снаряда.
    /// Является базовым объектом в паттерне "Декоратор", предоставляя исходные характеристики.
    /// </summary>
    public class StandartBullet : IBullet
    {
        /// <summary>
        /// Возвращает базовый урон пули.
        /// </summary>
        /// <returns>10 единиц урона.</returns>
        public int GetDamage()
        {
            return 10;
        }

        /// <summary>
        /// Возвращает базовую скорость полета пули.
        /// </summary>
        /// <returns>8.0 единиц скорости.</returns>
        public float GetSpeed()
        {
            return 8.0f;
        }

        /// <summary>
        /// Возвращает стандартный коэффициент задержки стрельбы.
        /// </summary>
        /// <returns>Коэффициент 1.0f.</returns>
        public float GetCooldown()
        {
            return 1.0f;
        }

        /// <summary>
        /// Возвращает текущий объект, так как он является базовым и не содержит декораторов.
        /// </summary>
        /// <returns>Текущий экземпляр StandartBullet.</returns>
        public IBullet GetBase()
        {
            return this;
        }
    }
}