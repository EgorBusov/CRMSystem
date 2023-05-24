using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.Models.ResourceModels;

namespace CRMWebForWorker.ApiInteraction.ApiResources
{
    public class ApiResources
    {
        private readonly ResourceRequests _resourceRequests;

        public ApiResources(string baseUrl)
        {
            _resourceRequests = new ResourceRequests(new HttpClient(), baseUrl );
            InitializeResources();
        }

        #region Поля и свойства

        #region Header

        private Header _header;
        public Header Header 
        { 
            get { return _header; }
            private set { _header = value; }
        }

        #endregion

        #region Buttons

        private IEnumerable<Button> _buttons;

        public IEnumerable<Button> Buttons
        {
            get { return _buttons; }
            private set { _buttons = value; }
        }

        #endregion

        #region Contacts

        private IEnumerable<ContactToView> _contactsToView;

        public IEnumerable<ContactToView> ContactsToView
        {
            get { return _contactsToView; }
            private set { _contactsToView = value; }
        }

        #endregion

        #region MainPage

        private MainPageContent _mainPageContent;

        public MainPageContent MainPageContent
        {
            get { return _mainPageContent; }
            private set { _mainPageContent = value; }
        }

        #endregion

        #region OurInformation

        private OurInformationToView _ourInformationToView;

        public OurInformationToView OurInformationToView
        {
            get { return _ourInformationToView; }
            private set { _ourInformationToView = value; }
        }

        #endregion

        #endregion

        #region Методы

        /// <summary>
        /// инициализация экземпляра
        /// </summary>
        private async void InitializeResources()
        {
            await GetResources();
        }

        /// <summary>
        /// Конвертирует коллекцию ContactModel в ContactToView
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ContactToView>> ConvertContactToView(IEnumerable<ContactModel> models)
        {
            if (models.ToList().Count == 0)
            {
                return new List<ContactToView>();
            }
            List<ContactToView> contactToViews = new List<ContactToView>();
            foreach (var model in models)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ContactToView view = new ContactToView() { Id = model.Id, Link = model.Link};
                    await model.Picture.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var imageBase64 = Convert.ToBase64String(imageBytes);

                    view.Picture = imageBase64;
                    contactToViews.Add(view);
                }
            }

            return contactToViews;
        }

        /// <summary>
        /// Конвертирует OurInformationModel в OurInformationToView
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private async Task<OurInformationToView> ConvertOurInformationToView(OurInformationModel model)
        {
            if (model == null)
            {
                return new OurInformationToView();
            }
            OurInformationToView view = new OurInformationToView()
                { Id = model.Id, Address = model.Address, Fax = model.Fax, Telephone = model.Telephone };

            using (var memoryStream = new MemoryStream())
            {
                await model.Picture.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var imageBase64 = Convert.ToBase64String(imageBytes);

                view.Picture = imageBase64;
            }

            return view;
        }

        /// <summary>
        /// Получение ресурсов
        /// </summary>
        public async Task GetResources()
        {
            Header = await _resourceRequests.GetPhraseRequest();
            Buttons = await _resourceRequests.GetButtonsRequest();
            ContactsToView = await ConvertContactToView(await _resourceRequests.GetContactModelsRequest());
            MainPageContent = await _resourceRequests.GetMainPageRequest();
            OurInformationToView = await ConvertOurInformationToView(await _resourceRequests.GetInformationModelRequest());
        }

        #endregion

    }
}
