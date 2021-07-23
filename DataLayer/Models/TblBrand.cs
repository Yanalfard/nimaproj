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
        [Required(ErrorMessage = "نام  را وارد کنید")]
        [MaxLength(100, ErrorMessage = "نام محصول را معتبر وارد کنید")]
        [MinLength(3, ErrorMessage = "نام محصول را معتبر وارد کنید")]
        public string Name { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(TblProduct.Brand))]
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
