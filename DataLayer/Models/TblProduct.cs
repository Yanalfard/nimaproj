using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblProduct")]
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblOfflineOrders = new HashSet<TblOfflineOrder>();
            TblOrderDetails = new HashSet<TblOrderDetail>();
            TblProductCommentRels = new HashSet<TblProductCommentRel>();
            TblProductImageRels = new HashSet<TblProductImageRel>();
            TblProductKeywordRels = new HashSet<TblProductKeywordRel>();
            TblProductPropertyRels = new HashSet<TblProductPropertyRel>();
            TblRates = new HashSet<TblRate>();
            TblSpecialOffers = new HashSet<TblSpecialOffer>();
        }

        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string MainImage { get; set; }
        public long PriceBeforeDiscount { get; set; }
        public string DescriptionShortHtml { get; set; }
        public string DescriptionLongHtml { get; set; }
        public int? CatagoryId { get; set; }
        public long PriceAfterDiscount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [StringLength(500)]
        public string SearchText { get; set; }
        public bool IsFractional { get; set; }
        public int BrandId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOfflineOrder { get; set; }

        [ForeignKey(nameof(CatagoryId))]
        [InverseProperty(nameof(TblCatagory.TblProducts))]
        public virtual TblCatagory Catagory { get; set; }
        [InverseProperty(nameof(TblOfflineOrder.Product))]
        public virtual ICollection<TblOfflineOrder> TblOfflineOrders { get; set; }
        [InverseProperty(nameof(TblOrderDetail.Product))]
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
        [InverseProperty(nameof(TblProductCommentRel.Product))]
        public virtual ICollection<TblProductCommentRel> TblProductCommentRels { get; set; }
        [InverseProperty(nameof(TblProductImageRel.Product))]
        public virtual ICollection<TblProductImageRel> TblProductImageRels { get; set; }
        [InverseProperty(nameof(TblProductKeywordRel.Product))]
        public virtual ICollection<TblProductKeywordRel> TblProductKeywordRels { get; set; }
        [InverseProperty(nameof(TblProductPropertyRel.Product))]
        public virtual ICollection<TblProductPropertyRel> TblProductPropertyRels { get; set; }
        [InverseProperty(nameof(TblRate.Product))]
        public virtual ICollection<TblRate> TblRates { get; set; }
        [InverseProperty(nameof(TblSpecialOffer.Product))]
        public virtual ICollection<TblSpecialOffer> TblSpecialOffers { get; set; }
    }
}
