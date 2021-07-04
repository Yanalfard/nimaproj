using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblImage")]
    public partial class TblImage
    {
        public TblImage()
        {
            TblProductImageRels = new HashSet<TblProductImageRel>();
        }

        [Key]
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int? AlbumId { get; set; }
        public bool IsImage { get; set; }

        [InverseProperty(nameof(TblProductImageRel.Image))]
        public virtual ICollection<TblProductImageRel> TblProductImageRels { get; set; }
    }
}
