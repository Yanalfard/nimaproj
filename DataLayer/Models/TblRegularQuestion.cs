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
        [Required(ErrorMessage = "سوال اجباری میباشد")]
        [StringLength(2000, ErrorMessage = "سوال مناسب وارد کنید")]
        public string Question { get; set; }
        [Required(ErrorMessage = "جواب اجباری میباشد")]
        [StringLength(2000, ErrorMessage = "جواب مناسب وارد کنید")]
        public string Answer { get; set; }
    }
}
