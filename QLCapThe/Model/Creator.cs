using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLCapThe.Model
{
    [Table("Creator")]
    public partial class Creator
    {
        [Key]
        [Column("CreatorID")]
        public int CreatorId { get; set; }
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Password { get; set; }
        [StringLength(255)]
        public string? Username { get; set; }
    }
}
