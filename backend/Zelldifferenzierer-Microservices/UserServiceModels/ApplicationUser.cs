using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using UserServiceModels.Relationships;

namespace UserServiceModels
{
    public enum UserType
    {
        User, Administrator

    }
    public class ApplicationUser : IdentityUser
    {
        public string Token { get; set; }

        [StringLength(50)]
        [PersonalData]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(50)]
        [ProtectedPersonalData]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(50)]
        [ProtectedPersonalData]
        public string LastName { get; set; }

        [Required]
        public UserType Type { get; private set; } //ROLES?

        public /*virtual*/  ICollection<UserGroups> Groups { get; set; }

        public ApplicationUser(string title, string firstName, string lastName, string email, string identificationNumber) 
        {
            Title = title ?? throw new ArgumentNullException(nameof(Title));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(FirstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(LastName));
            Email = email;
            Type = UserType.User;
            UserName = identificationNumber;
        }

        #region adminCtor
        public ApplicationUser(string title, string firstName, string lastName, string email, string identificationNumber, UserType type)
        {
            Title = title ?? throw new ArgumentNullException(nameof(Title));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(FirstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(LastName));
            Email = email;
            UserName = identificationNumber;
            Type = type;
        }
        #endregion

        public ApplicationUser() { }


        private bool IdentificationNumberIsValid(string identificationNumberToCheck)
        {
            return OnlyContainsNumbers(identificationNumberToCheck) && identificationNumberToCheck.Length > 0 &&
                   identificationNumberToCheck.Length <= 32;
        }

        public bool OnlyContainsNumbers(string toTest)
        {
            return int.TryParse(toTest, out var i);
        }

    }
}
