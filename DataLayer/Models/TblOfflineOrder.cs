using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblOfflineOrder")]
    public partial class TblOfflineOrder
    {
        [Key]
        public int OnlineOrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(50)]
        public string Body { get; set; }
        public int ClientId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateSubmited { get; set; }
        public bool IsRead { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(TblClient.TblOfflineOrders))]
        public virtual TblClient Client { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(TblProduct.TblOfflineOrders))]
        public virtual TblProduct Product { get; set; }
    }
}
