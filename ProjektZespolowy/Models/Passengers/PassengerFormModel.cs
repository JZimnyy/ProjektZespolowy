using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.Passengers
{
    #region PassegnerFormModel
    public class PassengerFormModel
    {
        [Required]
        [Display(Name = "Imie")]
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
    #endregion

    #region PassengerViewModel
    public class PassengerViewModel
    {
        [Required]
        public Guid publicId { get; set; }
        [Required]
        [Display(Name = "Imię")]
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
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
    #endregion
}