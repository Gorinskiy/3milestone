using Microsoft.AspNetCore.Mvc;
using Project.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class RegisterModel : IValidatableObject
    {
        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress]
        [Remote(action: "VerifyEmail", controller: "Account")]
        [ValidEmailDomain(allowedDomain: "gmail.com", ErrorMessage = "Email domain must be gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string number = "Not Contain Number";
            string upperLetter = "Not Contain Uppercase Letter";
            if (Password.Contains("0") || Password.Contains("1") || Password.Contains("2") || Password.Contains("3") || Password.Contains("4") || Password.Contains("5") || Password.Contains("6") || Password.Contains("7") || Password.Contains("8") || Password.Contains("9"))
            {
                number = "";
            }
            bool hasUppercase = !Password.Equals(Password.ToLower());
            if (hasUppercase)
            {
                upperLetter = "";
            }
            Console.WriteLine("NUMBER: " + number);
            Console.WriteLine("UPPERLETTER: " + upperLetter);
            if (!number.Equals("") || !upperLetter.Equals(""))
            {
                yield return new ValidationResult("Your password is incorrect because: " + number + " " + upperLetter, new[] { "Password" });
            }
        }
    }
}
