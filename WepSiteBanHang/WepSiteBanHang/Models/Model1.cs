namespace WepSiteBanHang.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<BINHLUAN> BINHLUANs { get; set; }
        public virtual DbSet<CHITIETDONDATHANG> CHITIETDONDATHANGs { get; set; }
        public virtual DbSet<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
        public virtual DbSet<DONDATHANG> DONDATHANGs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<LIENHE> LIENHEs { get; set; }
        public virtual DbSet<LOAISANPHAM> LOAISANPHAMs { get; set; }
        public virtual DbSet<LOAITHANHVIEN> LOAITHANHVIENs { get; set; }
        public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }
        public virtual DbSet<NHASANXUAT> NHASANXUATs { get; set; }
        public virtual DbSet<PHIEUNHAP> PHIEUNHAPs { get; set; }
        public virtual DbSet<QUANLY> QUANLies { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<THANHVIEN> THANHVIENs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BINHLUAN>()
                .Property(e => e.MABL)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BINHLUAN>()
                .Property(e => e.MATHANHVIEN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<BINHLUAN>()
                .Property(e => e.MASP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETDONDATHANG>()
                .Property(e => e.MACHITIETDDH)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETDONDATHANG>()
                .Property(e => e.MADDH)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETDONDATHANG>()
                .Property(e => e.MASP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETDONDATHANG>()
                .Property(e => e.SOLUONG)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETPHIEUNHAP>()
                .Property(e => e.MACHITIETPN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETPHIEUNHAP>()
                .Property(e => e.MAPN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETPHIEUNHAP>()
                .Property(e => e.MASP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CHITIETPHIEUNHAP>()
                .Property(e => e.SOLUONGNHAP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DONDATHANG>()
                .Property(e => e.MADDH)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DONDATHANG>()
                .Property(e => e.TINHTRANGGIAO)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DONDATHANG>()
                .Property(e => e.DATHANHTOAN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DONDATHANG>()
                .Property(e => e.MATHANHVIEN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DONDATHANG>()
                .Property(e => e.UUDAI)
                .HasPrecision(38, 0);

            modelBuilder.Entity<DONDATHANG>()
                .HasMany(e => e.CHITIETDONDATHANGs)
                .WithRequired(e => e.DONDATHANG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.MATHANHVIEN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.MAKH)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LIENHE>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LIENHE>()
                .Property(e => e.TRANGTHAI)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LIENHE>()
                .Property(e => e.DIENTHOAI)
                .IsUnicode(false);

            modelBuilder.Entity<LOAISANPHAM>()
                .Property(e => e.MALOAISP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LOAITHANHVIEN>()
                .Property(e => e.MALOAITV)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LOAITHANHVIEN>()
                .Property(e => e.UUDAI)
                .HasPrecision(38, 0);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.MANCC)
                .HasPrecision(38, 0);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.DIENTHOAI)
                .IsUnicode(false);

            modelBuilder.Entity<NHASANXUAT>()
                .Property(e => e.MANSX)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PHIEUNHAP>()
                .Property(e => e.MAPN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PHIEUNHAP>()
                .Property(e => e.MANCC)
                .HasPrecision(38, 0);

            modelBuilder.Entity<PHIEUNHAP>()
                .Property(e => e.DAXOA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<QUANLY>()
                .Property(e => e.TENTK)
                .IsUnicode(false);

            modelBuilder.Entity<QUANLY>()
                .Property(e => e.PASS)
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MASP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.DONGIA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.SOLUONGTON)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.LUOTXEM)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.LUOTBINHCHON)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.SOLANMUA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MOI)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.DAXOA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MANCC)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MANSX)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MALOAISP)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SANPHAM>()
                .HasMany(e => e.CHITIETDONDATHANGs)
                .WithRequired(e => e.SANPHAM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<THANHVIEN>()
                .Property(e => e.MATHANHVIEN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<THANHVIEN>()
                .Property(e => e.MALOAITV)
                .HasPrecision(38, 0);

            modelBuilder.Entity<THANHVIEN>()
                .Property(e => e.SDT)
                .IsUnicode(false);
        }
    }
}
