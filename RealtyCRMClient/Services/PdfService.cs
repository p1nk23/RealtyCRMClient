using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;

namespace RealtyCRMClient.Services
{
    public class PdfService
    {
        public void GeneratePdf(string content, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Путь к файлу не может быть пустым.");

                var directory = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using var writer = new PdfWriter(fileName);
                using var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Указываем шрифт DejaVuSans.ttf
                string fontPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Fonts", "DejaVuSans.ttf");

                // Создаем шрифт с поддержкой Unicode
                var font = PdfFontFactory.CreateFont(fontPath, "Identity-H", true);

                // Добавляем текст с использованием шрифта
                var paragraph = new Paragraph(content)
                    .SetFont(font)
                    .SetFontSize(10);

                document.Add(paragraph);
                document.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при генерации PDF: {ex.Message}", ex);
            }
        }
    }
}
