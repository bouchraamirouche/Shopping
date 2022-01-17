using System.ComponentModel.DataAnnotations;

namespace Shopping.Infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png" };
                bool result = extension.Any(x => extension.EndsWith(x));
                if (!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }
               
            }
            return ValidationResult.Success;
        }

        private String GetErrorMessage()
        {
            return "Allowed extensions are : jpeg and png.";
        }
    }
}
