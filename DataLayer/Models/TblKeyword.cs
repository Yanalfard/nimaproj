using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblKeyword")]
    public partial class TblKeyword
    {
        public TblKeyword()
        {
            TblBlogKeywordRels = new HashSet<TblBlogKeywordRel>();
            TblProductKeywordRels = new HashSet<TblProductKeywordRel>();
        }

        [Key]
        public int KeywordId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty(nameof(TblBlogKeywordRel.Keyword))]
        public virtual ICollection<TblBlogKeywordRel> TblBlogKeywordRels { get; set; }
        [InverseProperty(nameof(TblProductKeywordRel.Keyword))]
        public virtual ICollection<TblProductKeywordRel> TblProductKeywordRels { get; set; }
    }
}
