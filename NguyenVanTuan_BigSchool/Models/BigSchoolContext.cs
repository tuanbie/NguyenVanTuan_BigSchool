using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NguyenVanTuan_BigSchool.Models
{
    public partial class BigSchoolContext : DbContext
    {
        public BigSchoolContext()
            : base("name=BigSchoolContext")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Following> Followings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Course)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
