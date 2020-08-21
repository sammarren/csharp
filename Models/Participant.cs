using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId {get;set;}
        public int UserId {get;set;}
        public int DojoActivityId {get;set;}

        //Nagivational property - User Fan
        public User Guest {get;set;}

        //Navigational prop - match
        public DojoActivity Event {get;set;}
    }
}