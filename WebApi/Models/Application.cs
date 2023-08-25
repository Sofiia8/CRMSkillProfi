using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Application
    {
        public int Id { get; set; }
        public DateTime Datetime {get; set;}
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        [ForeignKey("ApplicationStatus")]
        public int ApplicationStatusId { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
    }
}
