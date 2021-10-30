using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblCatagory", Schema = "dbo")]
    public partial class TblCatagory
    {
        public TblCatagory()
        {
            InverseParent = new HashSet<TblCatagory>();
            TblBlogs = new HashSet<TblBlog>();
            TblProducts = new HashSet<TblProduct>();
        }

        [Key]
        public int CatagoryId { get; set; }
        [Required(ErrorMessage = "نام  را وارد کنید")]
        [MaxLength(100, ErrorMessage = "نام محصول را معتبر وارد کنید")]
        [MinLength(3, ErrorMessage = "نام محصول را معتبر وارد کنید")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsOnFirstPage { get; set; }
        public bool IsBlog { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(TblCatagory.InverseParent))]
        public virtual TblCatagory Parent { get; set; }
        [InverseProperty(nameof(TblCatagory.Parent))]
        public virtual ICollection<TblCatagory> InverseParent { get; set; }
        [InverseProperty(nameof(TblBlog.Catagory))]
        public virtual ICollection<TblBlog> TblBlogs { get; set; }
        [InverseProperty(nameof(TblProduct.Catagory))]
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
