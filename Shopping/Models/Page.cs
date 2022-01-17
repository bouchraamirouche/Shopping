using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Page
    {
        public int Id { get; set; }
        [Required , MinLength(2,ErrorMessage ="minimum length is two car")]
        public String Title { get; set; }
        
        public String Slug { get; set; }
        [Required, MinLength(10, ErrorMessage = "minimum length is ten carachters")]
        public String Content { get; set; }
        public int Sorting { get; set; }
        

    }
}
