using System.Collections.Generic;

namespace Website.Models
{
    public class ContactsViewModel
    {
        public string PostIndex { get; set; }
        //codeFiled = 1, descriptionField = "PostIndex", valueField = "123456"
        public string City { get; set; }
        //codeFiled = 2, descriptionField = "City", valueField = "Москва"
        public string Street { get; set; }
        //codeFiled = 3, descriptionField = "Street", valueField = "Симоновский вал"
        public string House { get; set; }
        //codeFiled = 4, descriptionField = "House", valueField = "34"
        public string PhoneNumber { get; set; }
        //codeFiled = 5, descriptionField = "PhoneNumber", valueField = "+7(900)999-99-99"
        public string Fax { get; set; }
        //codeFiled = 6, descriptionField = "Fax", valueField = "8(495)456-78-90"
        public string Email { get; set; }
        //codeFiled = 7, descriptionField = "Email", valueField = "skillptofi@ya.ru"
        public string ImageId { get; set; }
        //codeFiled = 8, descriptionField = "ImageId", valueField = "address48756849648690954.png"
        public string VKReference { get; set; }
        //codeFiled = 9, descriptionField = "VKReference", valueField = "vk.com"
        public string TelegramReference { get; set; }
        //codeFiled = 10, descriptionField = "TelegramReference", valueField = "telegram.space"
        public string YoutubeReference { get; set; }
        //codeFiled = 11, descriptionField = "YoutubeReference", valueField = "www.youtube.com"
    }
}
