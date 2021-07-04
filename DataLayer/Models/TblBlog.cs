using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblBlog")]
    public partial class TblBlog
    {
        public TblBlog()
        {
            TblBlogCommentRels = new HashSet<TblBlogCommentRel>();
            TblBlogKeywordRels = new HashSet<TblBlogKeywordRel>();
        }

        [Key]
        public int BlogId { get; set; }
        [StringLength(200)]
        public string MainImage { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public string BodyHtml { get; set; }
        public int LikeCount { get; set; }
        public int ViewCount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }

        [InverseProperty(nameof(TblBlogCommentRel.Blog))]
        public virtual ICollection<TblBlogCommentRel> TblBlogCommentRels { get; set; }
        [InverseProperty(nameof(TblBlogKeywordRel.Blog))]
        public virtual ICollection<TblBlogKeywordRel> TblBlogKeywordRels { get; set; }
    }
}
