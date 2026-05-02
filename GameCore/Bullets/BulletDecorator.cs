using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Абстрактный базовый класс для декораторов снарядов.
    /// Реализует хранение обернутого объекта и управление временем действия эффекта.
    /// </summary>
    public abstract class BulletDecorator : IBullet
    {
        /// <summary> Ссылка на вложенный объект снаряда. </summary>
        protected IBullet _bullet;

        /// <summary> Оставшееся время действия данного улучшения в секундах. </summary>
        public float TimeLeft { get; set; } = 0;

        /// <summary> Возвращает истину, если время действия эффекта истекло. </summary>
        public bool IsExpired => TimeLeft <= 0;

        /// <summary>
        /// Доступ к внутреннему объекту снаряда. 
        /// </summary>
        public IBullet Inner
        {
            get => _bullet;
            set => _bullet = value;
        }

        /// <summary>
        /// Инициализирует декоратор, связывая его с существующим снарядом.
        /// </summary>
        /// <param name="bullet">Объект, который будет декорирован.</param>
        /// <param name="duration">Длительность действия эффекта.</param>
        public BulletDecorator(IBullet bullet, float duration)
        {
            _bullet = bullet;
            TimeLeft = duration;
        }

        /// <summary>
        /// Рекурсивно обновляет таймеры действия для всей цепочки декораторов.
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с предыдущего кадра.</param>
        public void UpdateTime(float deltaTime)
        {
            TimeLeft -= deltaTime;

            // Если внутри находится еще один декоратор, ешо время также обновляется
            if (_bullet is BulletDecorator innerDecorator)
            {
                innerDecorator.UpdateTime(deltaTime);
            }
        }

        /// <summary>
        /// Возвращает урон вложенного объекта. Может быть переопределено в наследниках.
        /// </summary>
        public virtual int GetDamage()
        {
            return _bullet.GetDamage();
        }

        /// <summary>
        /// Возвращает скорость вложенного объекта. Может быть переопределено в наследниках.
        /// </summary>
        public virtual float GetSpeed()
        {
            return _bullet.GetSpeed();
        }

        /// <summary>
        /// Возвращает коэффициент задержки вложенного объекта. Может быть переопределено в наследниках.
        /// </summary>
        public virtual float GetCooldown()
        {
            return _bullet.GetCooldown();
        }

        /// <summary>
        /// Рекурсивно вызывает GetBase у вложенных объектов, чтобы добраться до StandartBullet.
        /// </summary>
        public virtual IBullet GetBase()
        {
            return _bullet.GetBase();
        }
    }
}