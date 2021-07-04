using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblBlogCommentRel")]
    public partial class TblBlogCommentRel
    {
        [Key]
        public int BlogCommentRelId { get; set; }
        public int BlogId { get; set; }
        public int CommentId { get; set; }

        [ForeignKey(nameof(BlogId))]
        [InverseProperty(nameof(TblBlog.TblBlogCommentRels))]
        public virtual TblBlog Blog { get; set; }
        [ForeignKey(nameof(CommentId))]
        [InverseProperty(nameof(TblComment.TblBlogCommentRels))]
        public virtual TblComment Comment { get; set; }
    }
}
