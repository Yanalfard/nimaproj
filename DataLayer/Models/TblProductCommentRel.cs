using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblProductCommentRel")]
    public partial class TblProductCommentRel
    {
        [Key]
        public int ProductCommentRelId { get; set; }
        public int ProductId { get; set; }
        public int CommentId { get; set; }

        [ForeignKey(nameof(CommentId))]
        [InverseProperty(nameof(TblComment.TblProductCommentRels))]
        public virtual TblComment Comment { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(TblProduct.TblProductCommentRels))]
        public virtual TblProduct Product { get; set; }
    }
}
