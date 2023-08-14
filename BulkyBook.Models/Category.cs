using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100)]
        public int Displayorder { get; set; }

        public DateTime Createdatetime { get; set; } = DateTime.Now;
    }
}
