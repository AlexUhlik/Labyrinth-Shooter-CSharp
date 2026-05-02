using GameCore.Characters;
using GameCore.Map;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    /// <summary>
    /// Конкретная фабрика для создания врагов типа «Хаос».
    /// Генерирует противников с крайне высокими показателями здоровья и ценности, 
    /// выполняющих роль особо опасных юнитов.
    /// </summary>
    public class ChaosEnemyFactory : EnemyFactory
    {
        /// <summary>
        /// Инициализирует фабрику врагов Хаоса.
        /// </summary>
        /// <param name="map">Экземпляр карты лабиринта для валидации позиций.</param>
        /// <param name="players">Список игроков для исключения спавна в зоне видимости.</param>
        public ChaosEnemyFactory(LabyrinthMap map, List<Player> players)
            : base(map, players) { }

        /// <summary>
        /// Создает экземпляр врага Хаоса с максимальными характеристиками живучести и награды.
        /// </summary>
        /// <param name="x">Мировая координата по горизонтали.</param>
        /// <param name="y">Мировая координата по вертикали.</param>
        /// <returns>Объект Enemy с 250 ед. здоровья и уникальным визуальным оформлением.</returns>
        public override Enemy CreateEnemy(float x, float y)
        {
            return new Enemy(x, y)
            {
                // Максимальный запас здоровья. 
                // В пять раз превосходит легкого врага, что требует от игрока длительного огневого контакта.
                Health = 250,

                // Сбалансированная скорость. 
                // Достаточно быстр, чтобы представлять угрозу, но медленнее элитного врага.
                Speed = 3.0f,

                // Максимальная награда . 
                // Стимулирует игрока к риску ради получения высокого игрового счета.
                Score = 500,

                // Визуальный идентификатор типа врага (ярко-оранжевый оттенок).
                DisplayColor = System.Drawing.Color.FromArgb(255, 89, 0)
            };
        }
    }
}