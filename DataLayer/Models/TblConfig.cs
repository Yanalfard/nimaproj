using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblConfig", Schema = "dbo")]
    public partial class TblConfig
    {
        [Key]
        [StringLength(128)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
