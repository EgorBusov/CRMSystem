using CRMApi.Models;

namespace CRMApi.Interfaces
{
    /// <summary>
    /// Взаимодействие с бд касаемо заявок
    /// </summary>
    public interface IOrderData
    {
        /// <summary>
        /// Получение всех заявок
        /// </summary>
        /// <returns></returns>
        IEnumerable<Order> GetOrders();
        /// <summary>
        /// Добавление заявки
        /// </summary>
        /// <param name="order"></param>
        void AddOrder(Order order);
        /// <summary>
        /// Изменение статуса заявки
        /// </summary>
        /// <param name="status"></param>
        /// <param name="orderId"></param>
        void EditStatusOrder(string status, int orderId);
        /// <summary>
        /// Поиск заявки по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Order GetOrderById(int id);
    }
}
