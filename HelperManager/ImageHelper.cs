using System.Drawing;
using System.Security.Cryptography;

namespace HelperManager
{
  public static class ImageHelper
  {
    public static Image? FindAndLoadImage(string? pathFolder, string? fileName)
    {
      if (string.IsNullOrEmpty(pathFolder)) return null;
      if (string.IsNullOrEmpty(fileName)) return null;

      // Lấy tên file từ đường dẫn
      fileName = Path.GetFileName(fileName);
      string fullPath = Path.Combine(pathFolder, fileName);
      if (File.Exists(fullPath))
      {
        using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
          return Image.FromStream(fs);
        }
      }
      return null;
    }

    public static void SaveImage(string base64String, string outputPath)
    {
      try
      {
        // Nếu Base64 có tiền tố "data:image/png;base64,", cần loại bỏ
        if (base64String.Contains(","))
        {
          base64String = base64String.Substring(base64String.IndexOf(",") + 1);
        }

        byte[] imageBytes = Convert.FromBase64String(base64String);
        File.WriteAllBytes(outputPath, imageBytes);
        Console.WriteLine("Đã lưu ảnh thành công: " + outputPath);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Lỗi khi lưu ảnh: " + ex.Message);
      }
    }
  }
}
