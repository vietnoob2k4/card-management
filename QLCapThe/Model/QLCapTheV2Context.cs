using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QLCapThe.Model
{
    public partial class QLCapTheV2Context : DbContext
    {
        public QLCapTheV2Context()
        {
        }

        public QLCapTheV2Context(DbContextOptions<QLCapTheV2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<Creator> Creators { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

    }
}
