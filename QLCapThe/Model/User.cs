using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLCapThe.Model
{
    public partial class User
    {
        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(255)]
        public string? Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastUpdate { get; set; }
        [StringLength(50)]
        public string? Role { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Birthdate { get; set; }
    }
}
