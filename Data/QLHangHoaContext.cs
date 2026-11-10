using Microsoft.EntityFrameworkCore;
using SinhVien_MaSV.Models;

namespace SinhVien_MaSV.Data;

public class QLHangHoaContext : DbContext
{
    public QLHangHoaContext(DbContextOptions<QLHangHoaContext> options) : base(options) { }
    public DbSet<LoaiHang> LoaiHang => Set<LoaiHang>();
    public DbSet<HangHoa> HangHoa => Set<HangHoa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoaiHang>().ToTable("LoaiHang").HasKey(x => x.MaLoai);
        modelBuilder.Entity<HangHoa>().ToTable("HangHoa").HasKey(x => x.MaHang);
        modelBuilder.Entity<HangHoa>().Property(x => x.Gia).HasColumnType("numeric(18,0)");
        modelBuilder.Entity<HangHoa>().HasOne(x => x.LoaiHang)
            .WithMany(x => x.HangHoas).HasForeignKey(x => x.MaLoai);
    }
}
