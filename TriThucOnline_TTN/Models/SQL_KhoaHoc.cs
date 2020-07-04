namespace TriThucOnline_TTN.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SQL_KhoaHoc : DbContext
    {
        public SQL_KhoaHoc()
            : base("name=SQL_KhoaHoc")
        {
        }

        public virtual DbSet<CT_DONHANG> CT_DONHANG { get; set; }
        public virtual DbSet<DAUSACH> DAUSACHes { get; set; }
        public virtual DbSet<DONHANG> DONHANGs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<KHUYENMAI> KHUYENMAIs { get; set; }
        public virtual DbSet<NXB> NXBs { get; set; }
        public virtual DbSet<QUANTRIVIEN> QUANTRIVIENs { get; set; }
        public virtual DbSet<TACGIA> TACGIAs { get; set; }
        public virtual DbSet<KHOAHOC> KHOAHOCs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DAUSACH>()
                .Property(e => e.PicBook)
                .IsUnicode(false);

            modelBuilder.Entity<DONHANG>()
                .Property(e => e.MaKM)
                .IsUnicode(false);

            modelBuilder.Entity<DONHANG>()
                .HasMany(e => e.CT_DONHANG)
                .WithRequired(e => e.DONHANG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.PicUser)
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .HasMany(e => e.DONHANGs)
                .WithOptional(e => e.KHACHHANG)
                .WillCascadeOnDelete();

            modelBuilder.Entity<KHUYENMAI>()
                .Property(e => e.MaKM)
                .IsUnicode(false);

            modelBuilder.Entity<NXB>()
                .HasMany(e => e.DAUSACHes)
                .WithOptional(e => e.NXB)
                .WillCascadeOnDelete();

            modelBuilder.Entity<QUANTRIVIEN>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<TACGIA>()
                .Property(e => e.PicTG)
                .IsUnicode(false);

            modelBuilder.Entity<TACGIA>()
                .HasMany(e => e.DAUSACHes)
                .WithOptional(e => e.TACGIA)
                .WillCascadeOnDelete();

            modelBuilder.Entity<KHOAHOC>()
                .HasMany(e => e.DAUSACHes)
                .WithOptional(e => e.KHOAHOC)
                .WillCascadeOnDelete();
        }
    }
}
