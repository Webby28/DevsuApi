using SelectPdf;

namespace Core.Contracts.Helpers;
public class PdfGenerator
{
    public byte[] GeneratePdf(string htmlContent)
    {
        // Create a new PDF document
        PdfDocument document = new PdfDocument();

        // Create a new page
        PdfPage page = document.AddPage();

        // Create a HTML to PDF converter
        HtmlToPdf converter = new HtmlToPdf();

        // Convert the HTML content to PDF and save it to the document
        PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContent);

        // Save the PDF document to a byte array
        byte[] pdfBytes = pdfDocument.Save();

        // Clean up resources
        pdfDocument.Close();

        return pdfBytes;
    }
}