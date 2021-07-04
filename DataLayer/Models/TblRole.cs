using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataLayer.Models
{
    [Table("TblRole")]
    public partial class TblRole
    {
        public TblRole()
        {
            TblClients = new HashSet<TblClient>();
        }

        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Title { get; set; }

        [InverseProperty(nameof(TblClient.Role))]
        public virtual ICollection<TblClient> TblClients { get; set; }
    }
}
