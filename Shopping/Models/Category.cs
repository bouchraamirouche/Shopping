using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "minimum length is two car")]
        [RegularExpression(@"^[a-zA-Z-]+$",ErrorMessage ="allow only letters")]
        public String Name { get; set; }

        public String Slug { get; set; }
    
       
        public int Sorting { get; set; }
    }
}
