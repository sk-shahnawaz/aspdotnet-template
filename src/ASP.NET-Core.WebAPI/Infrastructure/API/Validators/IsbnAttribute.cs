using System.ComponentModel.DataAnnotations;

using ASP.NET.Core.WebAPI.Utilities;

namespace ASP.NET.Core.WebAPI.Infrastructure.API.Validators;

public class IsbnAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult(string.Format(AppResources.EmptyValueValidationMessage, validationContext.DisplayName));
        }
        char[] numbers = value.ToString().Replace("-", string.Empty).ToCharArray();
        if (numbers.Length == 10 || numbers.Length == 13)
        {
            if (numbers.Length == 10)
            {
                /* 
                    Validation logic of 10 character long ISBN:
                    {Sum of (NUMBER AT A PARTICULAR POSITION x (10 - POSITION))} when divided by 11 should return 0.    
                 */
                int sum = 0;
                int index = 0;
                Array.ForEach(numbers, number =>
                {
                    sum += number * (10 - index);
                    index++;
                });
                if (sum % 11 != 0)
                {
                    return new ValidationResult(string.Format(AppResources.InvalidValueValidationMessage, validationContext.DisplayName));
                }
            }
            else
            {
                /* 
                    Validation logic of 13 character long ISBN:
                    {Sum of (NUMBER AT A PARTICULAR POSITION x MULTIPLIER)} when divided by 10 should return 0.
                        where MULTIPLIER = 1, when POSTION is ODD, MULTIPLIER = 3, when POSITION is EVEN.
                 */
                var (sum, index, multiplier) = (0, 0, 1);   // C# 9
                Array.ForEach(numbers, number =>
                {
                    sum += (number * multiplier);
                    multiplier = multiplier == 1 ? 3 : 1;
                    index++;
                });
                if (sum % 10 != 0)
                {
                    return new ValidationResult(string.Format(AppResources.InvalidValueValidationMessage, validationContext.DisplayName));
                }
            }
        }
        else
        {
            return new ValidationResult(string.Format(AppResources.InvalidValueValidationMessage, validationContext.DisplayName));
        }
        return ValidationResult.Success;
    }
}