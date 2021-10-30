using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblProperty", Schema = "dbo")]
    public partial class TblProperty
    {
        public TblProperty()
        {
            TblProductPropertyRels = new HashSet<TblProductPropertyRel>();
        }

        [Key]
        public int PropertyId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }

        [InverseProperty(nameof(TblProductPropertyRel.Property))]
        public virtual ICollection<TblProductPropertyRel> TblProductPropertyRels { get; set; }
    }
}
