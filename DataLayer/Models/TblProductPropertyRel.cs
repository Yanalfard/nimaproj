using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblProductPropertyRel")]
    public partial class TblProductPropertyRel
    {
        [Key]
        public int ProductPropertyRelId { get; set; }
        public int ProductId { get; set; }
        public int PropertyId { get; set; }
        [Required]
        [StringLength(150)]
        public string Value { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(TblProduct.TblProductPropertyRels))]
        public virtual TblProduct Product { get; set; }
        [ForeignKey(nameof(PropertyId))]
        [InverseProperty(nameof(TblProperty.TblProductPropertyRels))]
        public virtual TblProperty Property { get; set; }
    }
}
