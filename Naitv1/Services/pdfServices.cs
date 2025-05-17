using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;

namespace Naitv1.Services
{
    public class pdfServices
    {
        private readonly IConverter _converter;

        public pdfServices(IConverter converter)
        {
            _converter = converter;

        }

        public byte[] GeneradorPdfHTML(String html)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = html,
                        WebSettings ={DefaultEncoding = "utf-8"}
                    }
                }
            };
            return _converter.Convert(doc); 

        }
    }
}
