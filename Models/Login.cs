using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class Login
    {
        [Required(ErrorMessage="Email address is required.")]        
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage="Password is required.")]        
        [DataType(DataType.Password)]
        public string LoginPassword {get;set;}
    }
}