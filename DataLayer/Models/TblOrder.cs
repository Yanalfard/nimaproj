using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblOrder")]
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
        }

        [Key]
        public int OrdeId { get; set; }
        public int? DiscountId { get; set; }
        public int ClientId { get; set; }
        public bool IsPayed { get; set; }
        public int FinalPrice { get; set; }
        [Required]
        [StringLength(500)]
        public string Address { get; set; }
        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }
        public int Status { get; set; }
        public int SendStatus { get; set; }
        public int? SendPrice { get; set; }
        public int PaymentStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateSubmited { get; set; }
        public bool IsFractional { get; set; }
        [StringLength(150)]
        public string SentNo { get; set; }
        public int? SentId { get; set; }
        public long? FractionalPartPayed { get; set; }
        [StringLength(15)]
        public string IpV4 { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(TblClient.TblOrders))]
        public virtual TblClient Client { get; set; }
        [ForeignKey(nameof(DiscountId))]
        [InverseProperty(nameof(TblDiscount.TblOrders))]
        public virtual TblDiscount Discount { get; set; }
        [ForeignKey(nameof(SentId))]
        [InverseProperty(nameof(TblPostOption.TblOrders))]
        public virtual TblPostOption Sent { get; set; }
        [InverseProperty(nameof(TblOrderDetail.FinalOrder))]
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
