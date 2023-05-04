using CRMApi.Models;

namespace CRMApi.Interfaces
{
    /// <summary>
    /// Взаимодействие с бд касаемо услуг
    /// </summary>
    public interface IServiceData
    {
        /// <summary>
        /// Получение списка услуг
        /// </summary>
        /// <returns></returns>
        IEnumerable<Service> GetServices();
        /// <summary>
        /// Получение услуги по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Service GetServiceById(int id);
        /// <summary>
        /// Редактирование услуги
        /// </summary>
        /// <param name="s"></param>
        void EditService(Service s);
        /// <summary>
        /// Удаление услуги
        /// </summary>
        /// <param name="s"></param>
        void DeleteService(Service s);
        /// <summary>
        /// Добавление услуги
        /// </summary>
        /// <param name="s"></param>
        void AddService(Service s);
    }
}
