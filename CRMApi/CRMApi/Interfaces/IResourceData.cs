using CRMApi.Models.ResourceModels;

namespace CRMApi.Interfaces
{
    /// <summary>
    /// Взаимодействие c вспомогательными ресурсами приложения
    /// </summary>
    public interface IResourceData
    {
        /// <summary>
        /// Возвращает случайную фразу для header
        /// </summary>
        Task<Header> GetPhrase();
        /// <summary>
        /// Возвращает коллекцию с кнопками меню для отображения
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Button>> GetButtons();
        /// <summary>
        /// Изменение кнопки меню
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        Task<bool> EditButton(Button b);
        /// <summary>
        /// Получение всех контактов
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ContactPath>> GetContacts();
        /// <summary>
        /// Добавление контакта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddContact(ContactModel model);
        /// <summary>
        /// Удаление контакта
        /// </summary>
        /// <param name="idContact"></param>
        /// <returns></returns>
        Task DeleteContact(int idContact);
        /// <summary>
        /// Получение контента для главной страницы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MainPageContent> GetMainPage(int id = 1);
        /// <summary>
        /// Изменение контента на главной странице
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<bool> EditMainPage(MainPageContent content);
        /// <summary>
        /// Получение информации о компании
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OurInformationPath> GetInformationModel(int id = 1);
        /// <summary>
        /// Изменение информации о компании
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> EditInformationModel(OurInformationModel model);

        Task<MemoryStream> GetPicture(string fileName);
    }
}
