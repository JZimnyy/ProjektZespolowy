using ProjektZespolowy.Models.AirPort;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.AirRoutes
{
    public class AirRouteFormModel
    {
        [Required]
        public int AirLineId { get; set; }
        [Required]
        public string StartAirportCode { get; set; }
        [Required]
        public string FinishAirportCode { get; set; }
    }

    public class AirRouteViewModel
    {
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        public virtual AirLine AirLine { get; set; }
        [Required]
        public virtual Airport StartAirport { get; set; }
        [Required]
        public virtual Airport FinishAirport { get; set; }
    }
}