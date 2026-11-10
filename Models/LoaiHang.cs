using System.ComponentModel.DataAnnotations;
namespace SinhVien_MaSV.Models;
public class LoaiHang
{
    [Key] public int MaLoai { get; set; }
    [Required, StringLength(50)] public string TenLoai { get; set; } = "";
    public ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
