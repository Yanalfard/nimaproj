using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblProductKeywordRel")]
    public partial class TblProductKeywordRel
    {
        [Key]
        public int ProductKeywordRelId { get; set; }
        public int ProductId { get; set; }
        public int KeywordId { get; set; }

        [ForeignKey(nameof(KeywordId))]
        [InverseProperty(nameof(TblKeyword.TblProductKeywordRels))]
        public virtual TblKeyword Keyword { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(TblProduct.TblProductKeywordRels))]
        public virtual TblProduct Product { get; set; }
    }
}
