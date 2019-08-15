using System;
namespace NeilsonAssessment.Api.Models
{
    public class ContactPersonModel
    {
        public ContactPersonModel()
        {
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int ReminderPreference { get; set; }
        public string AdditionalComments { get; set; }
    }
}
