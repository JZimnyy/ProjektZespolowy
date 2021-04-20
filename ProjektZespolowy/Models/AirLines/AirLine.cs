using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models
{
    public class AirLine
    {
        [Required]
        public int AirLineId { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        [Display(Name="Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Kraj")]
        public string Country { get; set; }
        [Required]
        [Display(Name="Link do strony")]
        [Url]
        public string LinkToPage { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public virtual List<AirRoute> Routes { get; set; }
    }
}