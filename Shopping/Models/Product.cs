using Shopping.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "minimum length is two car")]

        public String Name { get; set; }

        public String Slug { get; set; }

        [Required, MinLength(10, ErrorMessage = "minimum length is ten car")]

        public String Description { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Range(1,int.MaxValue, ErrorMessage ="you must choose a category")]
        public int CategoryId { get; set; }

        public String Image { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [NotMapped]

        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
