using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblPostOption")]
    public partial class TblPostOption
    {
        public TblPostOption()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        [Key]
        public int PostOptionId { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public int Price { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [InverseProperty(nameof(TblOrder.Sent))]
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
