using System.Collections.Generic;

namespace Website.Models
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        //codeFiled = 1, descriptionField = "FieldHeader", valueField = "#SkillProfi"
        public string FieldSlogan { get; set; }
        //codeFiled = 2, descriptionField = "FieldSlogan", valueField = "Расширяем границы"
        public string ButtonBegin { get; set; }
        //codeFiled = 3, descriptionField = "ButtonBegin", valueField = "Начать"
        public string FieldFillForm { get; set; }
        //codeFiled = 4, descriptionField = "FieldFillForm", valueField = "Заполните форму"
        public string FieldName { get; set; }
        //codeFiled = 5, descriptionField = "FieldName", valueField = "Имя"
        public string FieldEmail { get; set; }
        //codeFiled = 6, descriptionField = "FieldEmail", valueField = "Email"
        public string FieldDescription { get; set; }
        //codeFiled = 7, descriptionField = "FieldDescription", valueField = "Описание"
        public string ButtonPost { get; set; }
        //codeFiled = 8, descriptionField = "ButtonPost", valueField = "Отправить"
        public string ButtonMain { get; set; }
        //codeFiled = 9, descriptionField = "ButtonMain", valueField = "Главная"
        public string ButtonServices { get; set; }
        //codeFiled = 10, descriptionField = "ButtonServices", valueField = "Услуги"
        public string ButtonProjects { get; set; }
        //codeFiled = 11, descriptionField = "ButtonProjects", valueField = "Проекты"
        public string ButtonBlog { get; set; }
        //codeFiled = 12, descriptionField = "ButtonBlog", valueField = "Блог"
        public string ButtonContacts { get; set; }
        //codeFiled = 13, descriptionField = "ButtonContacts", valueField = "Контакты"
        public string ButtonLogin { get; set; }
    }
}
