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

        private IEnumerable<ContactPath> _contactsToView;

        public IEnumerable<ContactPath> ContactsToView
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

        private OurInformationPath _ourInformationToView;

        public OurInformationPath OurInformationToView
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
        /// Получение ресурсов
        /// </summary>
        public async Task GetResources()
        {
            Header = await _resourceRequests.GetPhraseRequest();
            Buttons = await _resourceRequests.GetButtonsRequest();
            ContactsToView = await _resourceRequests.GetContactsRequest();
            MainPageContent = await _resourceRequests.GetMainPageRequest();
            OurInformationToView = await _resourceRequests.GetInformationRequest();
        }

        #endregion

    }
}
