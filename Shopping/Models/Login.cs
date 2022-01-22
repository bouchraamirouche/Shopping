using System.ComponentModel.DataAnnotations;


namespace Shopping.Models
{
    
        public class Login
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "4")]
            public string Password { get; set; }

            public string ReturnUrl { get; set; }
        
    }
}
