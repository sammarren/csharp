using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required(ErrorMessage="Please enter a first name")]
        [MinLength(2,ErrorMessage="First name must be atleast 2 characters")]
        public string FirstName {get;set;}

        [Required(ErrorMessage="Please enter a last name")]
        [MinLength(2, ErrorMessage="Last name must be atleast 2 characters")]
        public string LastName {get;set;}

        [Required(ErrorMessage="Please enter an email address")]
        [EmailAddress]
        public string Email {get;set;}

        [Required(ErrorMessage="Please enter a password")]
        [MinLength(8, ErrorMessage="Password must be atleast 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[!*@#$%^&+=]).*$", ErrorMessage = "Password must have 1 character, 1 number, and 1 special character.")]
        public string Password {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Please confirm your password")]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<DojoActivity> MyActivities {get;set;}

        //Navigation property - Many to many - User can be a fan at many soccermatches
        public List<Participant> AttendingActivities {get;set;}

    }
}