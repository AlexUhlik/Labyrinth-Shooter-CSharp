using GameCore.Characters;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    /// <summary>
    /// Конкретная фабрика для создания «элитных» противников.
    /// Производит юнитов с повышенными характеристиками скорости и здоровья, 
    /// представляющих повышенную угрозу для игрока.
    /// </summary>
    public class EliteEnemyFactory : EnemyFactory
    {
        /// <summary>
        /// Инициализирует фабрику элитных врагов.
        /// </summary>
        /// <param name="map">Экземпляр карты для проверки доступности ячеек.</param>
        /// <param name="players">Список игроков для соблюдения дистанции при появлении.</param>
        public EliteEnemyFactory(LabyrinthMap map, List<Player> players)
            : base(map, players) { }

        /// <summary>
        /// Создает экземпляр элитного противника с высокими показателями скорости и ценности.
        /// </summary>
        /// <param name="x">Мировая координата X.</param>
        /// <param name="y">Мировая координата Y.</param>
        /// <returns>Объект Enemy с удвоенным здоровьем и высокой скоростью передвижения.</returns>
        public override Enemy CreateEnemy(float x, float y)
        {
            return new Enemy(x, y)
            {
                //Увеличенный запас здоровья, что делает врага вдвое живучее обычного.
                Health = 100,
                Armor = 30,

                // Более высокая скорость.
                Speed = 4.0f,

                // Награда за уничтожение. Отражает сложность противника.
                Score = 300,

                // Визуальный идентификатор типа врага (темно-синий оттенок).
                DisplayColor = System.Drawing.Color.FromArgb(51, 179, 255)
            };
        }
    }
}