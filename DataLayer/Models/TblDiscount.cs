using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblDiscount")]
    public partial class TblDiscount
    {
        public TblDiscount()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        [Key]
        public int DiscountId { get; set; }
        public int Discount { get; set; }
        public int Count { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ValidTill { get; set; }

        [InverseProperty(nameof(TblOrder.Discount))]
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
