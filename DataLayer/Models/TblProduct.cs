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
            TblProductPropertyRels = new HashSet<TblProductPropertyRel>();
            TblSpecialOffers = new HashSet<TblSpecialOffer>();
        }

        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "نام محصول را وارد کنید")]
        [MaxLength(100, ErrorMessage = "نام محصول را معتبر وارد کنید")]
        [MinLength(3, ErrorMessage = "نام محصول را معتبر وارد کنید")]
        public string Name { get; set; }
        public string MainImage { get; set; }
        [Required(ErrorMessage = "لطفا قیمت محصول را وارد کنید")]
        public long? Price { get; set; }
        [Required(ErrorMessage = "لطفا توضیحات کوتاه محصول را وارد کنید")]
        public string DescriptionShortHtml { get; set; }
        [Required(ErrorMessage = "لطفا توضیحات کامل محصول را وارد کنید")]
        public string DescriptionLongHtml { get; set; }
        public int? CatagoryId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [StringLength(500)]
        [Required(ErrorMessage = "متن جستجو را وارد کنید")]
        public string SearchText { get; set; }
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
        [InverseProperty(nameof(TblProductPropertyRel.Product))]
        public virtual ICollection<TblProductPropertyRel> TblProductPropertyRels { get; set; }
        [InverseProperty(nameof(TblSpecialOffer.Product))]
        public virtual ICollection<TblSpecialOffer> TblSpecialOffers { get; set; }
    }
}
