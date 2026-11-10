using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SinhVien_MaSV.Models;

public class HangHoa
{
    [Key] public int MaHang { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại hàng hợp lệ")]
    [Display(Name = "Loại hàng")] public int MaLoai { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên hàng")]
    [StringLength(50, ErrorMessage = "Tên hàng tối đa 50 ký tự")]
    [Display(Name = "Tên hàng")] public string TenHang { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập giá")]
    [Range(typeof(decimal), "100", "5000", ErrorMessage = "Giá phải nằm trong khoảng 100 đến 5000")]
    [Display(Name = "Giá")] public decimal? Gia { get; set; }
    [StringLength(100)] [ImageExtension] [Display(Name = "Ảnh")] public string? Anh { get; set; }
    [ValidateNever] public LoaiHang? LoaiHang { get; set; }
}

public sealed class ImageExtensionAttribute : ValidationAttribute
{
    private static readonly string[] Extensions = [".jpg", ".png", ".gif", ".tiff"];
    public override bool IsValid(object? value)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString())) return true;
        var ext = Path.GetExtension(value.ToString());
        return Extensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
    }
    public override string FormatErrorMessage(string name) =>
        $"{name} phải có đuôi .jpg, .png, .gif hoặc .tiff.";
}
