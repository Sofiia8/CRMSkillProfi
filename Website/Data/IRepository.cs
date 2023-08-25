using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Data
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetItems(string jwt);
        Task<IEnumerable<T>> GetItemsFromToDates(DateTime dateTimeFrom, DateTime dateTimeTo, string jwt);        
        Task<HttpStatusCode> PostNewItem(T application);
        Task<HttpStatusCode> EditRecord(int id, string status, string jwt);
    }
}
