using GameCore.Bullets;
using System;

namespace GameCore
{
    /// <summary>
    /// Базовый абстрактный класс для всех живых сущностей в игре.
    /// Определяет общую логику движения, получения урона и визуальной индикации попаданий.
    /// </summary>
    public abstract class Unit : GameObject
    {
        /// <summary> Текущие очки здоровья юнита. </summary>
        public int Health { get; set; }

        /// <summary> Текущие очки брони юнита. </summary>
        public int Armor { get; set; }

        /// <summary> Скорость перемещения юнита. </summary>
        public float Speed { get; set; }

        /// <summary> Угол поворота юнита в радианах. </summary>
        public float Rotation { get; set; } = 0f;

        // Вычисляемые свойства направления на основе угла поворота
        public float DirectionX => (float)Math.Cos(Rotation);
        public float DirectionY => (float)Math.Sin(Rotation);

        /// <summary> Флаг, указывающий на то, что юнит находится в состоянии "вспышки" от урона. </summary>
        public bool IsDamaged { get; private set; } = false;

        /// <summary> Текущий тип снаряда врага. </summary>
        public IBullet CurrentBullet { get; set; }

        private float _damageFlashTimeLeft = 0f;
        private const float DamageFlashDuration = 0.1f;

        /// <summary> Событие, вызываемое при смерти юнита. Передает объект юнита и ID убийцы. </summary>
        public event Action<Unit, int> OnDied;

        /// <summary>
        /// Инициализирует новый экземпляр юнита.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        /// <param name="size">Размер коллизии.</param>
        public Unit(float x, float y, float size) : base(x, y, size)
        {
        }

        /// <summary>
        /// Изменяет позицию юнита на заданные смещения.
        /// </summary>
        public void Move(float dx, float dy)
        {
            Position += new Point(dx, dy);
        }

        /// <summary>
        /// Устанавливает угол поворота юнита в сторону вектора (dx, dy).
        /// </summary>
        public void SetDirection(float dx, float dy)
        {
            Rotation = (float)Math.Atan2(dy, dx);
        }

        /// <summary>
        /// Выполняет расчет урона, распределяя его между броней и здоровьем.
        /// </summary>
        /// <param name="damage">Входящий урон.</param>
        /// <param name="attackerId">Идентификатор атакующего.</param>
        public virtual void TakeDamage(int damage, int attackerId)
        {
            int armorDamage = Math.Min(damage, Armor);
            int healthDamage = damage - armorDamage;

            Armor -= armorDamage;
            Health -= healthDamage;

            StartDamageFlash();

            if (Health <= 0)
            {
                Health = 0;
                IsActive = false; 
                OnDied?.Invoke(this, attackerId);
            }
        }

        /// <summary>
        /// Перегрузка метода получения урона, принимающая объект пули.
        /// </summary>
        /// <param name="bullet">Снаряд, попавший в юнита.</param>
        public void TakeDamage(Bullet bullet)
        {
            TakeDamage(bullet.GetDamage(), bullet.OwnerId);
        }

        /// <summary>
        /// Активирует флаг повреждения для визуального рендерера.
        /// </summary>
        private void StartDamageFlash()
        {
            IsDamaged = true;
            _damageFlashTimeLeft = DamageFlashDuration;
        }

        /// <summary>
        /// Обновляет таймер визуальной вспышки урона.
        /// </summary>
        /// <param name="deltaTime">Время шага игры.</param>
        public void UpdateDamageFlash(float deltaTime)
        {
            if (!IsDamaged) return;

            _damageFlashTimeLeft -= deltaTime;

            if (_damageFlashTimeLeft <= 0)
            {
                IsDamaged = false;
            }
        }

        /// <summary>
        /// Абстрактный метод стрельбы, реализуемый в конкретных классах Player и Enemy.
        /// </summary>
        /// <returns>Созданный объект снаряда.</returns>
        public abstract Bullet Shoot();
    }
}