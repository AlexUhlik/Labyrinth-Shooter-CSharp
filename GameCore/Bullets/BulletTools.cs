using System;

namespace GameCore.Bullets
{
    /// <summary>
    /// Вспомогательный класс для работы с иерархией декораторов снарядов.
    /// Содержит методы для очистки истекших эффектов и проверки наличия конкретных модификаторов.
    /// </summary>
    public static class BulletTools
    {
        /// <summary>
        /// Рекурсивно удаляет из цепочки декораторов те объекты, время действия которых истекло.
        /// Гарантирует, что в стеке модификаторов останутся только активные эффекты.
        /// </summary>
        /// <param name="root">Корневой объект (текущая пуля игрока).</param>
        /// <returns>Обновленная цепочка декораторов или базовая пуля.</returns>
        public static IBullet Cleanup(IBullet root)
        {
            // Очистка с головы: если самый верхний декоратор истек,отбрасываем его и проверяем следующий по цепочке.
            while (root is BulletDecorator decorator && decorator.IsExpired)
            {
                root = decorator.Inner;
            }

            // Очистка в глубине: если корень активен, рекурсивно проверяем и чистим его внутренние слои.
            if (root is BulletDecorator current)
            {
                current.Inner = Cleanup(current.Inner);
            }

            return root;
        }

        /// <summary>
        /// Проверяет, активен ли в данный момент модификатор определенного типа в цепочке.
        /// Используется для логики отрисовки интерфейса или исключения дублирования эффектов.
        /// </summary>
        /// <typeparam name="T">Тип искомого декоратора.</typeparam>
        /// <param name="bullet">Объект для проверки.</param>
        /// <returns>True, если декоратор типа T найден и активен.</returns>
        public static bool IsDecoratorActive<T>(IBullet bullet)
        {
            // Если текущий объект соответствует искомому типу
            if (bullet is T) return true;

            // Если текущий объект — декоратор, рекурсивно ищем во вложенном объекте
            if (bullet is BulletDecorator decorator) return IsDecoratorActive<T>(decorator.Inner);

            // Если ничего не нашли
            return false;
        }
    }
}