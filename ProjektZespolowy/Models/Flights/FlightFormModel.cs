using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.Flights
{
    public class FlightFormModel
    {
        [Required]
        [Display(Name ="Trasa")]
        public int AirRouteId { get; set; }
        [Required]
        [Display(Name ="Liczba wolnych miejsc")]
        public int NumberOfFreeSeats { get; set; }
        [Required]
        [Display(Name ="Data odlotu")]
        [RegularExpression(@"(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2})",ErrorMessage ="Błędny format daty")]
        public string DepartureDate { get; set; }
        [Required]
        [Display(Name ="Data przylotu")]
        [RegularExpression(@"(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2})", ErrorMessage = "Błędny format daty")]

        public string ArrivalDate { get; set; }
        [Required]
        [Display(Name ="Cena")]
        public float Price { get; set; }

    }

    public class FlightViewModel
    {
        public Guid PublicId { get; set; }
        [Required]
        [Display(Name = "Trasa")]
        public int AirRouteId { get; set; }
        [Required]
        public virtual AirRoute AirRoute { get; set; }
        [Required]
        [Display(Name = "Liczba wolnych miejsc")]
        public int NumberOfFreeSeats { get; set; }
        [Required]
        [Display(Name = "Data odlotu")]
        public DateTime DepartureDate { get; set; }
        [Required]
        [Display(Name = "Data przylotu")]
        public DateTime ArrivalDate { get; set; }
        [Required]
        [Display(Name = "Cena")]
        public float Price { get; set; }
    }
}