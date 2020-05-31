using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotnetFlix.Database.Models
{
    public class Payment
    {
        [Required]
        [RegularExpression(@"^\p{L}{2,}([-]?\p{L}{2,})\s\p{L}{2,}([-\s]?\p{L}{2,}){0,3}$", ErrorMessage = "Name invalid")]
        public string OwnerName { get; set; }

        [Required]
        [RegularExpression(@"^(?:\d{4}\s){3}\d\d\d\d$", ErrorMessage = "Invalid Cardnumber")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid")]
        public int CVV { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Invalid")]
        public int Year { get; set; }

        [Required]
        [RegularExpression(@"^\d{1,2}$", ErrorMessage = "Invalid")]
        public int Month { get; set; }
    }
}
