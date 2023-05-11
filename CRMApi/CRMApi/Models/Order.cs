namespace CRMApi.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        public int ?Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string ?DateCreate { get; set; }
        public string ?Status { get; set; }
    }
}
