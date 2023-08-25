using System.Collections.Generic;

namespace WebApi.Models
{
    public class ApplicationStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public List<Application> Applications { get; set; }
    }
}
