using System.Text;

namespace HelperManager
{
  public static class TextHelper
  {
    public static string RemoveDiacritics(string text)
    {
      if (string.IsNullOrEmpty(text)) return string.Empty;

      var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
      var sb = new System.Text.StringBuilder();

      foreach (var c in normalized)
      {
        var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
        if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
        {
          sb.Append(c);
        }
      }
      var rs = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
      rs = rs.Replace('Đ', 'D').Replace('đ', 'd');
      return rs;
    }

    public static string RemoveVietnameseAccents(string text)
    {
      if (string.IsNullOrWhiteSpace(text))
        return text;

      // Normalize to FormD (separates accents)
      var formD = text.Normalize(System.Text.NormalizationForm.FormD);

      var sb = new StringBuilder();

      foreach (var ch in formD)
      {
        var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
        if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
        {
          sb.Append(ch);
        }
      }

      // Convert đ → d, Đ → D
      return sb.ToString()
               .Normalize(System.Text.NormalizationForm.FormC)
               .Replace('đ', 'd')
               .Replace('Đ', 'D');
    }


    public static List<string> SplitTextByMaxLength(string input, int maxLength = 40)
    {
      List<string> result = new List<string>();

      if (string.IsNullOrWhiteSpace(input))
      {
        result.Add("N/A");
        return result;
      }  


      input = input.Trim();
      string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

      string currentLine = "";

      foreach (string word in words)
      {
        // Nếu từ quá dài > maxLength thì phải cắt cứng
        if (word.Length > maxLength)
        {
          // đẩy dòng hiện tại vào trước
          if (!string.IsNullOrWhiteSpace(currentLine))
          {
            result.Add(currentLine);
            currentLine = "";
          }

          for (int i = 0; i < word.Length; i += maxLength)
          {
            int length = Math.Min(maxLength, word.Length - i);
            result.Add(word.Substring(i, length));
          }

          continue;
        }

        // Nếu thêm từ này vẫn chưa vượt quá maxLength
        if (string.IsNullOrEmpty(currentLine))
        {
          currentLine = word;
        }
        else if ((currentLine + " " + word).Length <= maxLength)
        {
          currentLine += " " + word;
        }
        else
        {
          result.Add(currentLine);
          currentLine = word;
        }
      }

      // thêm dòng cuối
      if (!string.IsNullOrWhiteSpace(currentLine))
      {
        result.Add(currentLine);
      }

      return result;
    }
  }
}
