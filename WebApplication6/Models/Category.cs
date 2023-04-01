using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1,100)]
        public int Displayorder { get; set; }

        public DateTime Createdatetime { get; set; } = DateTime.Now;
    }
}
