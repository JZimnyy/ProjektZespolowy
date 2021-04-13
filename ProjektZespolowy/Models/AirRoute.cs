using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models
{
    public class AirRoute
    {
        [Required]
        public int AirRouteId { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        public int AirLineId { get; set; }
        [Required]
        public virtual AirLine AirLine { get; set; }
        [Required]
        public string StartAirportCode { get; set; }
        [Required]
        public string FinishAirportCode { get; set; }



    }
}