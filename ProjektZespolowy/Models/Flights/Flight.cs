using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models
{
    public class Flight
    {
        [Required]
        public int FlightId { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        public int AirRouteId { get; set; }
        [Required]
        public virtual AirRoute AirRoute { get; set; }
        [Required]
        public int NumberOfFreeSeats { get; set; }
        [Required]
        public DateTime DepartureDate { get; set; }
        [Required]
        public DateTime ArrivalDate { get; set; }
        [Required]
        public float Price { get; set; }
    }
}