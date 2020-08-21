using System;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Validations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime)
            {
                DateTime check = (DateTime)value;
                if(DateTime.Now.Date < check.Date)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Activies must take place in the future.");
                }
            }
            else 
            {
                return new ValidationResult("Please enter a valid date.");
            }
        }
    }
}