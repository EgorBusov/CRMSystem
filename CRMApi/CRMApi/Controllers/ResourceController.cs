using CRMApi.Interfaces;
using CRMApi.Models.ResourceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceData _resourceData;

        public ResourceController(IResourceData resourceData)
        {
            _resourceData = resourceData;
        }
        
        #region Phrases
        /// <summary>
        /// Получение главной фразы в header 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetPhrase")]
        public async Task<Header> GetPhrase()
        {
            return await _resourceData.GetPhrase();
        }

        #endregion

        #region Buttons
        /// <summary>
        /// Получение кнопок для клавного меню
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetButtons")]
        public async Task<IEnumerable<Button>> GetButtons()
        {
            return await _resourceData.GetButtons();
        }
        /// <summary>
        /// Изменение кнопки
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditButton")]
        public async Task<IActionResult> EditButton([FromBody]Button b)
        {
            try
            {
                await _resourceData.EditButton(b);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Contacts
        /// <summary>
        /// Получение контактов для страницы с контактами
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetContactModels")]
        public async Task<IEnumerable<ContactBytes>> GetContactModels()
        {
            return await _resourceData.GetContactModels();
        }
        /// <summary>
        /// Добавление контакта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact([FromForm]ContactModel model)
        {
            try
            {
                await _resourceData.AddContact(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                await _resourceData.DeleteContact(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region MainPage
        /// <summary>
        /// Получение информации для главной страницы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetMainPage")]
        public async Task<MainPageContent> GetMainPage()
        {
            try
            {
                return await _resourceData.GetMainPage();
            }
            catch
            {
                return new MainPageContent();
            }
        }
        /// <summary>
        /// Изменение информации на основной странице
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditMainPage")]
        public async Task<IActionResult> EditMainPage([FromBody] MainPageContent content)
        {
            try
            {
                await _resourceData.EditMainPage(content);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region OurInformation
        /// <summary>
        /// Получение информации о нас(Адрес, телефон и т.д.)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetInformationModel")]
        public async Task<OurInformationModel> GetInformationModel()
        {
            return await _resourceData.GetInformationModel();
        }

        /// <summary>
        /// Изменяет информацию о нас(Адрес, телефон и т.д.)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditInformationModel")]
        public async Task<IActionResult> EditInformationModel([FromForm] OurInformationModel model)
        {
            try
            {
                await _resourceData.EditInformationModel(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
