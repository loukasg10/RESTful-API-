using System.ComponentModel.DataAnnotations;
namespace MyApiProject.Models
{


    public class Department
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string OfficeLocation { get; set; }
    }
}