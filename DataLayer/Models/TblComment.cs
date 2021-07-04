using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblComment")]
    public partial class TblComment
    {
        public TblComment()
        {
            InverseParent = new HashSet<TblComment>();
            TblBlogCommentRels = new HashSet<TblBlogCommentRel>();
            TblProductCommentRels = new HashSet<TblProductCommentRel>();
        }

        [Key]
        public int CommentId { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string Body { get; set; }
        public bool IsValid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(TblClient.TblComments))]
        public virtual TblClient Client { get; set; }
        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(TblComment.InverseParent))]
        public virtual TblComment Parent { get; set; }
        [InverseProperty(nameof(TblComment.Parent))]
        public virtual ICollection<TblComment> InverseParent { get; set; }
        [InverseProperty(nameof(TblBlogCommentRel.Comment))]
        public virtual ICollection<TblBlogCommentRel> TblBlogCommentRels { get; set; }
        [InverseProperty(nameof(TblProductCommentRel.Comment))]
        public virtual ICollection<TblProductCommentRel> TblProductCommentRels { get; set; }
    }
}
