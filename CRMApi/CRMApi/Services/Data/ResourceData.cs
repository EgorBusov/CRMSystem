using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.BlogModels;
using CRMApi.Models.ProjectModels;
using CRMApi.Models.ResourceModels;
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
        public ResourceData(CRMSystemContext context, IPictureManager pictureManager)
        {
            _context = context;
            _pictureManager = pictureManager;
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
        public async Task<IEnumerable<ContactBytes>> GetContactModels()
        {
            List<Contact> contacts = await _context.Contacts.ToListAsync();
            //List<ContactModel> contactModels = new List<ContactModel>();
            List<ContactBytes> contactBytes = new List<ContactBytes>();
            //foreach (Contact contact in contacts)
            //{
            //    ContactModel contactModel = new ContactModel() { Id = contact.Id, Link = contact.Link, GuidPicture = contact.GuidPicture };
            //    byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory,
            //                                                             "Pictures", "ContactPictures", contact.GuidPicture));

            //    contactModel.Picture = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, contact.GuidPicture)
            //    {
            //        Headers = new HeaderDictionary(),
            //        ContentType = "image/jpeg"
            //    };
            //    contactModels.Add(contactModel);
            //}

            foreach (Contact contact in contacts)
            {
                ContactBytes contactByte = new ContactBytes() { Id = contact.Id, Link = contact.Link };
                contactByte.GuidPicture = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory,
                    "Pictures", "ContactPictures", contact.GuidPicture));
                contactBytes.Add(contactByte);
            }
            return contactBytes;
        }

        public async Task AddContact(ContactModel model)
        {
            if (model.Picture == null || model.Link == null)
            {
                throw new Exception("Обязательные поля не заполнены");}
            Contact contact = new Contact()
            {
                GuidPicture = await _pictureManager.SavePicture(model.Picture, @"Pictures\ContactPictures"),
                Link = model.Link,
            };
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteContact(int idContact)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(b => b.Id == idContact) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(contact.GuidPicture, @"Pictures\ContactPictures");
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
        public async Task<OurInformationModel> GetInformationModel(int id = 1)
        {
            OurInformation inf = await _context.OurInformation.FirstOrDefaultAsync(a => a.Id == id) ?? new OurInformation();
            OurInformationModel model = new OurInformationModel() { Address = inf.Address, Fax = inf.Fax, Id = inf.Id, 
                                                                    GuidPicture = inf.GuidPicture, Telephone = inf.Telephone };
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory,
                                                                        "Pictures", "OtherPictures", inf.GuidPicture));
            model.Picture = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, inf.GuidPicture)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            return model;
        }

        public async Task<bool> EditInformationModel(OurInformationModel model)
        {
            if (model.Picture == null || model.Address == null || model.Telephone == null ||
                model.Fax == null)
            {
                throw new Exception("Обязательные поля не заполнены");}
            OurInformation inf = await _context.OurInformation.FirstOrDefaultAsync(a => a.Id == model.Id) 
                                                                                ?? throw new Exception("Информация не найдена");
            await _pictureManager.DeletePicture(inf.GuidPicture, @"Pictures\OtherPictures");
            inf.GuidPicture = await _pictureManager.SavePicture(model.Picture, @"Pictures\OtherPictures");
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