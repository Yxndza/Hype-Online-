using System;
using System.ComponentModel.DataAnnotations;

namespace HypestoreFinal.Models
{
    public class Subscription : IEquatable<Subscription>
    {
        public int ID { get; set; }
        
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        
        [Display(Name = "How did you hear about us?")]
        [Required(ErrorMessage = "Please select how you heard about us")]
        public Source? Source { get; set; }

        [Display(Name = "Please specify (optional)")]
        public string SourceOther { get; set; }

        [Display(Name = "Reason for signing up (optional)")]
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        [Display(Name = "Subscribed at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateTime { get; set; }

        public bool Equals(Subscription other)
        {
            return EmailAddress == other.EmailAddress
                && Source == other.Source
                && SourceOther == other.SourceOther
                && Reason == other.Reason;
        }
    }
}