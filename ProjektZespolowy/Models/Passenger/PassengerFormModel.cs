using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models
{
    public class PassengerFormModel
    {
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "PESEL")]
        [MinLength(11)]
        [MaxLength(11)]
        public string PESEL { get; set; }
        [Required]
        [Display(Name = "Numer dokumentu")]
        public string DocumentSerial { get; set; }
    }
}