using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SinhVien_MaSV.Data;
using SinhVien_MaSV.Models;

namespace SinhVien_MaSV.Controllers;

public class HangHoaController : Controller
{
    private readonly QLHangHoaContext _db;
    private readonly IWebHostEnvironment _env;

    public HangHoaController(QLHangHoaContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategories();
        return View(new HangHoa());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HangHoa model, IFormFile? imageFile)
    {
        // MaLoai là int nên [Required] không bắt được trường hợp người dùng để mặc định = 0.
        if (model.MaLoai <= 0 || !await _db.LoaiHang.AnyAsync(x => x.MaLoai == model.MaLoai))
            ModelState.AddModelError(nameof(model.MaLoai), "Vui lòng chọn loại hàng tồn tại trong danh sách.");

        string? relativeImagePath = null;
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError("imageFile", "Vui lòng chọn ảnh hàng hóa.");
        }
        else
        {
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".png", ".gif", ".tiff" };
            if (!allowed.Contains(extension))
                ModelState.AddModelError("imageFile", "Ảnh phải có đuôi .jpg, .png, .gif hoặc .tiff.");
            else if (imageFile.Length > 5 * 1024 * 1024)
                ModelState.AddModelError("imageFile", "Dung lượng ảnh tối đa 5MB.");
            else
                relativeImagePath = $"uploads/{Guid.NewGuid():N}{extension}";
        }

        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(model);
        }

        var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var uploadDirectory = Path.Combine(webRoot, "uploads");
        Directory.CreateDirectory(uploadDirectory);
        var savedFile = Path.Combine(uploadDirectory, Path.GetFileName(relativeImagePath!));

        try
        {
            await using (var stream = new FileStream(savedFile, FileMode.CreateNew, FileAccess.Write))
                await imageFile!.CopyToAsync(stream);

            model.Anh = relativeImagePath;
            _db.HangHoa.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Thêm hàng hóa thành công.";
            return RedirectToAction("Index", "Home");
        }
        catch (DbUpdateException)
        {
            if (System.IO.File.Exists(savedFile))
                System.IO.File.Delete(savedFile);

            ModelState.AddModelError(string.Empty, "Không lưu được hàng hóa vào cơ sở dữ liệu. Hãy kiểm tra lại kết nối SQL Server, bảng HangHoa và mã loại hàng.");
            await LoadCategories();
            return View(model);
        }
        catch (IOException)
        {
            ModelState.AddModelError("imageFile", "Không thể lưu ảnh. Hãy kiểm tra quyền ghi thư mục wwwroot/uploads.");
            await LoadCategories();
            return View(model);
        }
    }

    private async Task LoadCategories()
    {
        var categories = await _db.LoaiHang.OrderBy(x => x.TenLoai).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "MaLoai", "TenLoai");
    }
}
