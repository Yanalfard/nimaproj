using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    public partial class TblContactU
    {
        [Key]
        public int ContactUsId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(15)]
        public string TellNo { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
    }
}
