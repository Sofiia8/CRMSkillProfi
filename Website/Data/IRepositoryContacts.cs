using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Data
{
    public interface IRepositoryContacts
    {
        Task<Dictionary<string, string[]>> GetItems();
        Task<Contacts> GetContactByCode(int code, string jwt);
        Task<HttpStatusCode> EditContactById(int id, string newValue, string jwt);
    }
}
