using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using Atum.Domain.Business;
using Atum.Domain.Common;
using WebMatrix.WebData;

namespace SurveyWeb.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("AtumSurveillanceContext")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class PersonViewModel
    {
        public Person Person { get; set; }
        public string UserId { get; set; }
        public string Id { get { return this.Person.Id; } set { this.Person.Id = value; } }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get { return this.Person.LastName; } set { this.Person.LastName = value; } }
        
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get { return this.Person.FirstName; } set { this.Person.FirstName = value; } }

        [Display(Name = "Middle Name")]
        public string MiddleName { get { return this.Person.MiddleName; } set { this.Person.MiddleName = value; } }

        [Display(Name = "Suffix")]
        public string Suffix { get { return this.Person.Suffix; } set { this.Person.Suffix = value; } }
        
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get { return this.Person.Email; } set { this.Person.Email = value; } }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get { return this.Person.PhoneNumber; } set { this.Person.PhoneNumber = value; } }
        
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get { return this.Person.JobTitle; } set { this.Person.JobTitle = value; } }
        
        [Required]
        [Display(Name = "Industry")]
        public string Industry { get { return this.Person.Hospital.Industry; } set { this.Person.Hospital.Industry = value; } }
        
        public Address Address { get { return this.Person.Address; } set { this.Person.Address = value; } }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get { return this.Person.DateOfBirth; } set { this.Person.DateOfBirth = value; } }
        
        [Required]
        [DataType(DataType.Text)]
        public string CompanyName { get { return this.Person.Hospital.Name; } set { this.Person.Hospital.Name = value; } }
        
        public Hospital Company { get { return this.Person.Hospital; } set { this.Person.Hospital = value; } }

        public PersonViewModel()
        {
            this.Person = new Person { Hospital = new Hospital() };
        }

        public PersonViewModel(Person person)
        {
            this.Person = person;
        }
    }

    public class LostPasswordModel
    {
        [Required(ErrorMessage = "We need your email to send you a reset link!")]
        [Display(Name = "Your account email")]
        [EmailAddress(ErrorMessage = "Not a valid email--what are you trying to do here?")]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "New password and confirmation does not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnToken { get; set; }
    }

    public class InvitePersonModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Domain")]
        [DataType(DataType.Text)]
        public string Domain { get; set; }
    }
}
