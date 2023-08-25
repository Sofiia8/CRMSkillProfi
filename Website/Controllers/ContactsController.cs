using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class ContactsController : Controller
    {
        private IRepositorySettings _repositorySettings;
        private IRepositoryContacts _repositoryContacts;

        public ContactsController(IRepositorySettings repositorySettings, IRepositoryContacts repositoryContacts)
        {
            _repositorySettings = repositorySettings;
            _repositoryContacts = repositoryContacts;
        }
        public async Task<IActionResult> Contacts()
        {
            var result = await GetContacts();
            if (result == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
            }
            return View(result);
        }
        public async Task<ContactsSettingViewModel> GetContacts()
        {
            var settings = await _repositorySettings.GetMainSettings();
            if (settings == null)
            {
                return null;
            }
            var contactSetView = new ContactsSettingViewModel();
            var fields = typeof(ContactsSettingViewModel).GetProperties();
            foreach (var field in fields)
            {
                if (field.Name == "Contacts")
                    continue;
                field?.SetValue(contactSetView, settings[field.Name]);
            }

            ContactsViewModel contactsViewModel = new ContactsViewModel();
            var fieldsContacts = typeof(ContactsViewModel).GetProperties();

            var allContacts = await _repositoryContacts.GetItems();
            if (allContacts == null)
            {
                return null;
            }
            foreach (var field in fieldsContacts)
            {
                field?.SetValue(contactsViewModel, allContacts[field.Name][1]);
            }
            contactSetView.Contacts = contactsViewModel;
            return contactSetView;
        }
        public async Task<ContactsViewModel> GetContactsAdmin()
        {
            ContactsViewModel contactsViewModel = new ContactsViewModel();
            var fieldsContacts = typeof(ContactsViewModel).GetProperties();

            var allContacts = await _repositoryContacts.GetItems();
            if (allContacts == null)
            {
                return null;
            }
            foreach (var field in fieldsContacts)
            {
                field?.SetValue(contactsViewModel, allContacts[field.Name][1]);
            }
            return contactsViewModel;
        }
        [HttpGet]
        public async Task<IActionResult> EditContacts()
        {
            var curContacts = await GetContactsAdmin();
            if (curContacts == null)
                return null;
            return View("EditContactsForm", curContacts);
        }
        [HttpPost]
        public async Task<IActionResult> EditContacts(ContactsViewModel model)
        {
            var allContacts = await _repositoryContacts.GetItems();
            if (allContacts == null)
            {
                return null;
            }
            var fieldsContacts = typeof(ContactsViewModel).GetProperties();
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");
            foreach (var field in fieldsContacts)
            {
                var newValue = field?.GetValue(model)?.ToString();
                if (newValue == null)
                    return RedirectToAction("NotSuccess", "Auth", new { errors = "Не может быть пустых полей" });
                if (allContacts[field.Name][1] != newValue)
                {
                    int code;
                    if (!int.TryParse(allContacts[field.Name][0], out code))
                        return RedirectToAction("NotSuccess", "Auth", new { errors = "Ошибка редактирования" });
                    var contact = await _repositoryContacts.GetContactByCode(code, jwt);
                    if (contact == null)
                    {
                        return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
                    }
                    var result = 
                        await _repositoryContacts.EditContactById(contact.Id, field.GetValue(model).ToString(), jwt);
                    if (result != HttpStatusCode.OK)
                        return RedirectToAction("NotSuccess", "Auth", new { errors = "Ошибка редактирования" });
                }
            }
            return RedirectToAction("ApplicationsAdmin", "Request");
        }
    }
}
