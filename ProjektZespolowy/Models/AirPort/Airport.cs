using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.AirPort
{
    
    public class Airport
    {
        [Required]
        public int AirportId { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        [Display(Name="Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Kod lotniska")]
        [Index(IsUnique = true)]
        [StringLength(3)]
        public string Code { get; set; }

        
    }
}