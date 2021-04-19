using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.AirLines
{
    public class AirLineFormModel
    {
        [Display(Name="Nazwa linii")]
        [Required]
        public string Name { get; set; }
        [Display(Name="Kraj")]
        public string Country { get; set; }
        [Url]
        [Required]
        [Display(Name= "Link do strony")]
        public string LinkToPage { get; set; } 
    }

    public class AirlineViewModel
    {
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Kraj")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Link do strony")]
        [Url]
        public string LinkToPage { get; set; }

        public virtual List<AirRoute> Routes { get; set; }
    }
}