using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinhVien_MaSV.Data;

namespace SinhVien_MaSV.Controllers;

public class HomeController : Controller
{
    private readonly QLHangHoaContext _db;
    private readonly IConfiguration _configuration;
    public HomeController(QLHangHoaContext db, IConfiguration configuration)
    { _db = db; _configuration = configuration; }

    public async Task<IActionResult> Index()
    {
        ViewBag.StudentName = _configuration["Student:Name"] ?? "Họ Tên Sinh Viên";
        ViewBag.LoaiHang = await _db.LoaiHang.OrderBy(x => x.MaLoai).ToListAsync();
        var products = await _db.HangHoa.Include(x => x.LoaiHang)
            .Where(x => x.Gia >= 100).OrderByDescending(x => x.MaHang).ToListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> ByCategory(int id)
    {
        var products = await _db.HangHoa.Include(x => x.LoaiHang)
            .Where(x => x.MaLoai == id && x.Gia >= 100)
            .OrderByDescending(x => x.MaHang).ToListAsync();
        return PartialView("_ProductList", products);
    }
}
