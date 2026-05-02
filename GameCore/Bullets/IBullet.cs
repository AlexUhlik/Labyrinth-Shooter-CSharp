using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Определяет интерфейс для всех типов снарядов в игре.
    /// Позволяет реализовать паттерн "Декоратор" для динамического изменения характеристик пули.
    /// </summary>
    public interface IBullet
    {
        /// <summary>
        /// Возвращает урон, наносимый данным типом снаряда.
        /// </summary>
        int GetDamage();

        /// <summary>
        /// Возвращает скорость полета снаряда.
        /// </summary>
        float GetSpeed();

        /// <summary>
        /// Возвращает коэффициент задержки между выстрелами.
        /// </summary>
        float GetCooldown();

        /// <summary>
        /// Позволяет получить базовый объект пули.
        /// Используется для снятия всех временных эффектов.
        /// </summary>
        IBullet GetBase();
    }
}