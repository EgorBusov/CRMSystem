using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.BlogModels;
using CRMApi.Models.ProjectModels;
using CRMApi.Models.ResourceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.Services.Data
{
    /// <summary>
    /// Ресурсы для отображения
    /// </summary>
    public class ResourceData : IResourceData
    {
        private readonly CRMSystemContext _context;
        private readonly IPictureManager _pictureManager;
        private readonly string baseUrl;
        public ResourceData(CRMSystemContext context, IPictureManager pictureManager, IConfiguration configuration)
        {
            _context = context;
            _pictureManager = pictureManager;
            baseUrl = configuration.GetValue<string>("BaseUrl:Url");
        }

        public async Task<MemoryStream> GetPicture(string fileName)
        {
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory,
                "Pictures", fileName));
            if (bytes.Length == 0)
            {
                throw new Exception("Файл не найден"); }
            return new MemoryStream(bytes);
        }

        #region Phrases
        public async Task<Header> GetPhrase()
        {
            List<Header> phrases = await _context.Phrases.ToListAsync();
            Random random = new Random();
            int r = random.Next(phrases.Count);
            return phrases[r];
        }
        #endregion

        #region Buttons
        public async Task<IEnumerable<Button>> GetButtons()
        {
            return await _context.Buttons.ToListAsync();
        }

        public async Task<bool> EditButton(Button b)
        {
            if (b.Id == null || b.Name == null || b.Text == null)
            {
                throw new Exception("Заполнены не все поля");}
            Button button = await _context.Buttons.FirstOrDefaultAsync(a => a.Id == b.Id) ?? throw new Exception("Кнопка не найдена");
            button.Text = b.Text;
            button.Name = b.Name;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Contacts
        public async Task<IEnumerable<ContactPath>> GetContacts()
        {
            List<Contact> contacts = await _context.Contacts.ToListAsync();
            List<ContactPath> contactPaths = new List<ContactPath>();
            
            foreach (Contact contact in contacts)
            {
                ContactPath contactByte = new ContactPath() { Id = contact.Id, Link = contact.Link };
                contactByte.Picture = $"{baseUrl}/Resource/GetPicture/{contact.GuidPicture}";
                contactPaths.Add(contactByte);
            }
            return contactPaths;
        }

        public async Task AddContact(ContactModel model)
        {
            if (model.Picture == null || model.Link == null)
            {
                throw new Exception("Обязательные поля не заполнены");}
            Contact contact = new Contact()
            {
                GuidPicture = await _pictureManager.SavePicture(model.Picture),
                Link = model.Link,
            };
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteContact(int idContact)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(b => b.Id == idContact) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(contact.GuidPicture);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region MainPage
        public async Task<MainPageContent> GetMainPage(int id = 1)
        {
            return await _context.MainPageContents.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Запись не найдена");
        }

        public async Task<bool> EditMainPage(MainPageContent content)
        {
            if (content.Id == 0 || content.Title == null || content.TitleForm == null || content.Button == null) 
            { throw new Exception("Обязательные поля не заполнены"); }
            var mainPageContent = await _context.MainPageContents.FirstOrDefaultAsync(c => c.Id == content.Id) 
                                                                                      ?? throw new Exception("Запись не найдена");
            mainPageContent.Button = content.Button;
            mainPageContent.Title = content.Title;
            mainPageContent.TitleForm = content.TitleForm;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region OurInformation
        public async Task<OurInformationPath> GetInformationModel(int id = 1)
        {
            OurInformation inf = await _context.OurInformation.FirstOrDefaultAsync(a => a.Id == id) ?? new OurInformation();
            OurInformationPath path = new OurInformationPath() { Address = inf.Address, Fax = inf.Fax, Id = inf.Id, 
                                                                    GuidPicture = inf.GuidPicture, Telephone = inf.Telephone };
            path.Picture = $"{baseUrl}/Resource/GetPicture/{inf.GuidPicture}";
            return path;
        }

        public async Task<bool> EditInformationModel(OurInformationModel model)
        {
            if (model.Address == null || model.Telephone == null ||
                model.Fax == null)
            {
                throw new Exception("Обязательные поля не заполнены");}
            OurInformation inf = await _context.OurInformation.FirstOrDefaultAsync(a => a.Id == model.Id) 
                                                                                ?? throw new Exception("Информация не найдена");
            if (model.Picture.Length != 0)
            {
                await _pictureManager.DeletePicture(inf.GuidPicture);
                inf.GuidPicture = await _pictureManager.SavePicture(model.Picture);
            }
            inf.Fax = model.Fax;
            inf.Id = model.Id;
            inf.Address = model.Address;
            inf.Telephone = model.Telephone;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}