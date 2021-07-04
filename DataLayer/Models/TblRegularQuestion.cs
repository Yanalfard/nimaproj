using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblRegularQuestion")]
    public partial class TblRegularQuestion
    {
        [Key]
        public int RegularQuestionId { get; set; }
        [Required]
        [StringLength(4000)]
        public string Question { get; set; }
        [Required]
        [StringLength(4000)]
        public string Answer { get; set; }
    }
}
