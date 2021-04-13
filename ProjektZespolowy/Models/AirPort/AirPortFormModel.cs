using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjektZespolowy.Models.AirPort
{
    public class AirPortFormModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Kod lotniska")]
        [Index(IsUnique = true)]
        [StringLength(3)]
        public string Code { get; set; }
    }

    public class AirPortViewModel
    {
        [Required]
        public Guid publicId { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Kod lotniska")]
        [Index(IsUnique = true)]
        [StringLength(3)]
        public string Code { get; set; }
        
        public virtual List<AirRoute> StartedRoutes { get; set; }
        public virtual List<AirRoute> FinishRoutes { get; set; }
    }
}