using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblBrand")]
    public partial class TblBrand
    {
        public TblBrand()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        [Key]
        public int BrandId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(TblProduct.Brand))]
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
