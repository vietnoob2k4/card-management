using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLCapThe.Model
{
    public partial class Card
    {
        public Card()
        {
            // Đặt thời gian mặc định cho ngày tạo và ngày hết hạn
            CreatedAt = DateTime.Now;
            ExpiryDate = CreatedAt?.AddYears(3);
        }

        [Key]
        [Column("CardID")]
        public Guid CardId { get; set; }

        [Column("UserID")]
        public int? UserId { get; set; }

        [Column("CreatorID")]
        public int? CreatorId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; } = "Valid"; // Default status is "Valid"

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("CreatorId")]
        public virtual Creator? Creator { get; set; }

        // Method to check and update the status
        public void CheckAndUpdateStatus()
        {
            if (ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now)
            {
                Status = "Expired";
            }
        }
    }
}
