using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblOrderDetail", Schema = "dbo")]
    public partial class TblOrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public long Price { get; set; }
        public int? FinalOrderId { get; set; }

        [ForeignKey(nameof(FinalOrderId))]
        [InverseProperty(nameof(TblOrder.TblOrderDetails))]
        public virtual TblOrder FinalOrder { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(TblProduct.TblOrderDetails))]
        public virtual TblProduct Product { get; set; }
    }
}
