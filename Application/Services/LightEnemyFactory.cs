using GameCore.Characters;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    /// <summary>
    /// Конкретная фабрика для создания «легких» противников.
    /// Настраивает базовые характеристики врага: здоровье, скорость и цвет визуализации.
    /// </summary>
    public class LightEnemyFactory : EnemyFactory
    {
        /// <summary>
        /// Инициализирует фабрику легких врагов, передавая зависимости в базовый класс.
        /// </summary>
        /// <param name="map">Карта лабиринта для проверки позиций спавна.</param>
        /// <param name="players">Список игроков для расчета безопасного расстояния.</param>
        public LightEnemyFactory(LabyrinthMap map, List<Player> players)
            : base(map, players) { }

        /// <summary>
        /// Создает экземпляр слабого противника с предустановленными параметрами.
        /// </summary>
        /// <param name="x">Мировая координата появления по оси X.</param>
        /// <param name="y">Мировая координата появления по оси Y.</param>
        /// <returns>Объект Enemy с низким запасом здоровья и средней скоростью.</returns>
        public override Enemy CreateEnemy(float x, float y)
        {
            return new Enemy(x, y)
            {
                // Запас здоровья. 50 единиц делает врага более уязвимым.
                Health = 50,
                Armor = 10,

                // Скорость передвижения. 2.0f — базовая скорость для создания динамики.
                Speed = 2.0f,

                // Количество очков, начисляемых игроку за победу над этим врагом.
                Score = 100,

                // Визуальный идентификатор типа врага (светло-зеленый оттенок).
                DisplayColor = System.Drawing.Color.FromArgb(77, 230, 77)
            };
        }
    }
}