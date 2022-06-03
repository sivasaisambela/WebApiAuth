using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiAuth.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [Range(10.0,20000.0,ErrorMessage ="Minimum Price is 10.00")]
        public double PricePerUnit { get; set; }

        [Required]
        [Range(10.0, 20000.0, ErrorMessage = "Minimum Price is 10.00")]
        public double OpenPrice { get; set; }

        [Required]
        public double High { get; set; }

        [Required]
        public double Low { get; set; }

        [Required]
        public double Close { get; set; }

        public string Type { get; set; }
    }
}