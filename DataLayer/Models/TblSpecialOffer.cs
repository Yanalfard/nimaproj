using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblSpecialOffer")]
    public partial class TblSpecialOffer
    {
        [Key]
        public int SpecialOfferId { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ValidTill { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(TblProduct.TblSpecialOffers))]
        public virtual TblProduct Product { get; set; }
    }
}
