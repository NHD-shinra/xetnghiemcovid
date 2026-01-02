using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SAR.DAL.Entities
{
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<CONGTY> CONGTY { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIEN { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CONGTY>()
                .HasMany(e => e.NHANVIEN)
                .WithRequired(e => e.CONGTY)
                .WillCascadeOnDelete(false);
        }
    }
}
