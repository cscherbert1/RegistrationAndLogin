using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

//remove .Extended from after ...Models
namespace RegistrationAndLogin.Models
{
    //this class is for the validation of all users during the login process. 
    //Do not make changes to the computer-generated User class because any schema changes will override the validation added. 
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }

    }

    public class UserMetaData
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        public string FLastName { get; set; }

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Email Id is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name ="Date of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0: MM-dd-yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Password must be 6 chars in length")]
        public string Password { get; set; }

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be 6 chars in length")]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }



    }
}