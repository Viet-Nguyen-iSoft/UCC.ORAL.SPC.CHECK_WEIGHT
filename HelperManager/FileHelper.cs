using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace HelperManager
{
  public static class FileHelper
  {


    public static void MergePdfFiles(string[] pdfFiles, string outputPdfPath)
    {
      using var outputPdf = new PdfDocument();

      foreach (var file in pdfFiles)
      {
        using var inputPdf = PdfReader.Open(file, PdfDocumentOpenMode.Import);

        for (int i = 0; i < inputPdf.Pages.Count; i++)
        {
          outputPdf.AddPage(inputPdf.Pages[i]);
        }
      }

      outputPdf.Save(outputPdfPath);
    }
  }
}
