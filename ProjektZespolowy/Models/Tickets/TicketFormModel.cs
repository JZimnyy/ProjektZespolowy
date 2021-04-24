using ProjektZespolowy.Models.Passengers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.Tickets
{
    public class TicketFormModel
    {
        [Required]
        [Display(Name ="Pasażer")]
        public int PassengerId { get; set; }
        [Required]
        [Display(Name ="Lot")]
        public int FlightId { get; set; }
    }

    public class TicketViewModel
    {
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        [Display(Name = "Pasażer")]
        public int PassengerId { get; set; }
        [Required]
        public virtual Passenger Passenger { get; set; }
        [Required]
        [Display(Name = "Lot")]
        public int FlightId { get; set; }
        [Required]
        public Flight Flight { get; set; }
    }
}