using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeltExam.Validations;

namespace BeltExam.Models
{
    public class DojoActivity
    {
        
        [Key]
        public int DojoActivityId {get;set;}

        [Required(ErrorMessage="Activity Name is required.")]
        public string ActivityName {get;set;}

        [Required(ErrorMessage="Date is required.")]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date {get;set;}

        [Required(ErrorMessage="Time is required.")]
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}

        [Required(ErrorMessage="Duration is required.")]
        public int DurationInt {get;set;}

        [Required(ErrorMessage="This field is required.")]
        public string DurationStr {get;set;}

        [Required(ErrorMessage="Description is required.")]
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public int UserId {get;set;}

        //Navigational Prop -- One to many 
        public User Coordinator {get;set;}

        //Navigation prop -- Many to many -- A match can have many fans
        // naming it attendees since Fans is what we are naming our Fan database
        public List<Participant> Attendees {get;set;}
    }
}