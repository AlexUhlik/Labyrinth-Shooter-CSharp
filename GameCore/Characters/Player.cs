using System;
using GameCore.Bullets;

namespace GameCore.Characters
{
    /// <summary>
    /// Представляет игрового персонажа, управляемого пользователем.
    /// Содержит параметры здоровья, боезапаса, счета и логику перезарядки.
    /// </summary>
    public class Player : Unit
    {
        // Константы базовых характеристик
        public const int MaxHealth = 100;
        public const int DefaultArmor = 50;
        private const float DefaultSpeed = 4.0f;
        private const int DefaultAmmunition = 45;
        private const int DefaultPlayerSize = 50;

        // Константы системы штрафов и счета
        private const int DeathScorePenalty = 500;
        private const int MinScore = 0;

        // Система задержки стрельбы
        private const float ShootCooldownTime = 0.3f;
        private float _shootTimer = 0f;

        /// <summary>
        /// Возвращает истину, если таймер перезарядки завершен и игрок может произвести выстрел.
        /// </summary>
        public bool CanShoot => _shootTimer <= 0;

        /// <summary>
        /// Текущее количество боеприпасов игрока.
        /// </summary>
        public int Ammunition { get; set; }

        /// <summary>
        /// Текущий счет игрока.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Уникальный идентификатор игрока.
        /// </summary>
        public int Id { get; }

        private readonly Point _startPos;

        /// <summary>
        /// Создает новый экземпляр игрока с заданным ID и начальными координатами.
        /// </summary>
        public Player(int id, float x, float y) : base(x, y, DefaultPlayerSize)
        {
            Health = MaxHealth;
            Armor = DefaultArmor;
            Speed = DefaultSpeed;
            Ammunition = DefaultAmmunition;
            Score = MinScore;
            Id = id;

            _startPos = new Point(x, y);
            CurrentBullet = new StandartBullet();
        }

        /// <summary>
        /// Обновляет таймер перезарядки оружия.
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с предыдущего кадра.</param>
        public void UpdateCooldown(float deltaTime)
        {
            if (_shootTimer > 0)
            {
                _shootTimer -= deltaTime;
            }
        }

        /// <summary>
        /// Сбрасывает таймер перезарядки, учитывая модификатор текущего снаряда.
        /// </summary>
        public void ResetShootTimer()
        {
            _shootTimer = ShootCooldownTime * CurrentBullet.GetCooldown();
        }

        /// <summary>
        /// Возвращает игрока в начальную позицию и восстанавливает базовые характеристики.
        /// Применяет штраф к счету и сбрасывает улучшения оружия.
        /// </summary>
        public void Respawn()
        {
            Position = _startPos;
            Health = MaxHealth;
            Ammunition = DefaultAmmunition;
            _shootTimer = 0;

            Score = Math.Max(MinScore, Score - DeathScorePenalty);
            CurrentBullet = new StandartBullet(); 
        }

        /// <summary>
        /// Создает объект снаряда, если у игрока достаточно боеприпасов.
        /// </summary>
        /// <returns>Экземпляр Bullet или null, если патроны закончились.</returns>
        public override Bullet Shoot()
        {
            if (Ammunition <= 0) return null;

            Ammunition--;

            return new Bullet(
                Position.X,
                Position.Y,
                DirectionX,
                DirectionY,
                CurrentBullet,
                Id
            );
        }

        /// <summary>
        /// Общий метод обновления состояния игрока за один игровой такт.
        /// </summary>
        public void Update(float deltaTime)
        {
            UpdatePowerUps(deltaTime);      // Обновление времени действия призов
            UpdateDamageFlash(deltaTime);   // Визуализация получения урона
            UpdateCooldown(deltaTime);      // Таймер перезарядки
        }

        /// <summary>
        /// Обновляет состояние активных декораторов оружия и удаляет истекшие.
        /// </summary>
        public void UpdatePowerUps(float deltaTime)
        {
            if (CurrentBullet is BulletDecorator decorator)
            {
                decorator.UpdateTime(deltaTime);
                CurrentBullet = BulletTools.Cleanup(CurrentBullet);
            }
        }
    }
}