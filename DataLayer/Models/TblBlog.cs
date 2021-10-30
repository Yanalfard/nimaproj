﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblBlog", Schema = "dbo")]
    public partial class TblBlog
    {
        public TblBlog()
        {
            TblBlogCommentRels = new HashSet<TblBlogCommentRel>();
        }

        [Key]
        public int BlogId { get; set; }
        [StringLength(200)]
        public string MainImage { get; set; }
        [Required(ErrorMessage = "عنوان پست را وارد کنید")]
        [StringLength(200, ErrorMessage = "عنوان کوتاه تری وارد کنید")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا توضبحات  را وارد کنید")]
        [StringLength(500, ErrorMessage = "توضیحات کوتاه پست را وارد کنید")]
        public string Description { get; set; }
        [Required(ErrorMessage = "توضیحات کامل پست را وارد کنید")]
        public string BodyHtml { get; set; }
        public int LikeCount { get; set; }
        public int ViewCount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Required(ErrorMessage = "لطفا کلمات کلیدی  را وارد کنید")]
        [StringLength(500, ErrorMessage = "توضیحات کوتاه پست را وارد کنید")]
        public string SearchText { get; set; }
        public int? CatagoryId { get; set; }

        [ForeignKey(nameof(CatagoryId))]
        [InverseProperty(nameof(TblCatagory.TblBlogs))]
        public virtual TblCatagory Catagory { get; set; }
        [InverseProperty(nameof(TblBlogCommentRel.Blog))]
        public virtual ICollection<TblBlogCommentRel> TblBlogCommentRels { get; set; }
    }
}
