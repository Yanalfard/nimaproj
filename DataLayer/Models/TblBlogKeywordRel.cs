using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblBlogKeywordRel")]
    public partial class TblBlogKeywordRel
    {
        [Key]
        public int BlogKeywordRelId { get; set; }
        public int BlogId { get; set; }
        public int KeywordId { get; set; }

        [ForeignKey(nameof(BlogId))]
        [InverseProperty(nameof(TblBlog.TblBlogKeywordRels))]
        public virtual TblBlog Blog { get; set; }
        [ForeignKey(nameof(KeywordId))]
        [InverseProperty(nameof(TblKeyword.TblBlogKeywordRels))]
        public virtual TblKeyword Keyword { get; set; }
    }
}
