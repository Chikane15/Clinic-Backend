using System.ComponentModel.DataAnnotations;
using Assignment_05_03.Models;
using Assignment_05_03.Repository;

namespace Assignment_05_03.Customization.Validators
{
    public class LengthCustomization: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                string categoryId = value.ToString();
                if (categoryId.Length > 15)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
