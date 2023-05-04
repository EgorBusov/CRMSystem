using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models;

namespace CRMApi.Services.Data
{
    public class OrderData : IOrderData
    {
        private readonly CRMSystemContext _context;
        public OrderData(CRMSystemContext context) 
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.ToList() ?? new List<Order>();
        }
        public Order GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id) ?? throw new Exception("Заявка не найдена");
        }
        public void AddOrder(Order order)
        {
            if (order.Name == null || order.SurName == null || order.Email == null || order.Text == null) 
            {
                throw new Exception("Не все обязательные поля заполнены");
            }
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public void EditStatusOrder(string status, int orderId)
        {
            if (status == "Получена" || status == "В работе" || status == "Выполнена" || status == "Отклонена" || status == "Отменена")
            {
                Order order = _context.Orders.FirstOrDefault(o => o.Id == orderId) ?? throw new Exception("Заявка не найдена");
                order.Status = status;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Недопустимый статус");
            }
        }
    }
}
