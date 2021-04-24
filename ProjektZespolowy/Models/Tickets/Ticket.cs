using ProjektZespolowy.Models.Passengers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models
{
    public class Ticket
    {
        [Required]
        public int TicketId { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        public int PassengerId { get; set; }
        [Required]
        public virtual Passenger Passenger { get; set; }
        [Required]
        public int FlightId { get; set; }
        [Required]
        public Flight Flight { get; set; }
    }
}