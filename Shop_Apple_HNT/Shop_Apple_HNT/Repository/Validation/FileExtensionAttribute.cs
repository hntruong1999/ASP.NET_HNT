using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Shop_Apple_HNT.Repository.KtAnh
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extention = Path.GetExtension(file.FileName); //dhgfh.jpg
                string[] extentions = { "jpg", "png", "jpeg" };
                bool result = extentions.Any(x => extention.EndsWith(x));
                if(!result )
                {
                    return new ValidationResult("Cho phep jpg or png or jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
