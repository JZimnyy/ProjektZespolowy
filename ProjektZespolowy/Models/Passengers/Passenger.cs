using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.Passengers
{
    public class Passenger
    {
        [Required]
        public int PassengerId { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        [Display(Name ="Imię")]

        public string FirstName { get; set; }
        [Required]
        [Display(Name ="Nazwisko")]
        public string LastName { get; set; }
        [Required]
        [Display(Name ="PESEL")]
        [MinLength(11)]
        [MaxLength(11)]
        public string PESEL { get; set; }
        [Required]
        [Display(Name ="Numer dokumentu")]
        public string DocumentSerial { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public bool IsActive { get; set; }
        

    }
}