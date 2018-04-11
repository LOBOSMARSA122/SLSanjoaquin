using System;
using System.Collections.Generic;
using System.Text;

using iTextSharp.text.pdf;
using iTextSharp.text;

namespace NetPdf
{
    public class pdfPage : PdfPageEventHelper
    {
        /** The template with the total number of pages. */
        PdfTemplate templateNumPage;

        // This is the contentbyte object of the writerE:\SLSigesoft_SanJoaquin\Sigesoft\common\pdfPage.cs
        PdfContentByte cb;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        //I create a font object to use within my footer
        protected Font footer
        {
            get
            {
                // create a basecolor to use for the footer font, if needed.
                BaseColor grey = new BaseColor(0, 128, 128);
                Font font = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.BOLD, grey);
                return font;
            }
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            templateNumPage = writer.DirectContent.CreateTemplate(50, 50);
            cb = writer.DirectContent;
        }

        //override the OnStartPage event handler to add our header
        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            ////I use a PdfPtable with 1 column to position my header where I want it
            //PdfPTable headerTbl = new PdfPTable(1);

            ////set the width of the table to be the same as the document
            //headerTbl.TotalWidth = doc.PageSize.Width;

         

          
            ////I use an image logo in the header so I need to get an instance of the image to be able to insert it. I believe this is something you couldn't do with older versions of iTextSharp
            //Image logo = Image.GetInstance(@"Resources\Logo-Laboral-Medical1.jpg");
            ////Image logo = Image.GetInstance(HttpContext.Current.Server.MapPath("/images/logo.jpg"));

            ////I used a large version of the logo to maintain the quality when the size was reduced. I guess you could reduce the size manually and use a smaller version, but I used iTextSharp to reduce the scale. As you can see, I reduced it down to 7% of original size.
            //logo.ScalePercent(5f);

            ////create instance of a table cell to contain the logo
            //PdfPCell cell = new PdfPCell(logo);

            //cell.VerticalAlignment = Element.ALIGN_TOP;

            ////align the logo to the right of the cell
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;

            ////add a bit of padding to bring it away from the right edge
            //cell.PaddingLeft = 20;
            //cell.PaddingTop = -10;
           
            ////remove the border
            //cell.Border = PdfPCell.NO_BORDER;

            ////Add the cell to the table
            //headerTbl.AddCell(cell);

            ////write the rows out to the PDF output stream. I use the height of the document to position the table. Positioning seems quite strange in iTextSharp and caused me the biggest headache.. It almost seems like it starts from the bottom of the page and works up to the top, so you may ned to play around with this.
            ////headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 10), writer.DirectContent);
            //headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 0), writer.DirectContent);
        }

        //override the OnPageEnd event handler to add our footer
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            var rutaImg = Sigesoft.Common.Utils.GetApplicationConfigValue("imgFooter");
            var footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = doc.PageSize.Width;
            var widths = new[] { 100f };
            footerTbl.SetWidths(widths);
            footerTbl.HorizontalAlignment = Element.ALIGN_LEFT;

            Image jpg = Image.GetInstance(rutaImg);
            var imageCell = new PdfPCell(jpg);

            footerTbl.AddCell(imageCell);
             footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {

            //ColumnText.ShowTextAligned(templateNumPage, Element.ALIGN_LEFT, new Phrase((writer.PageNumber - 1).ToString()), 0, (document.BottomMargin + 10), 0);
            templateNumPage.BeginText();
            templateNumPage.SetFontAndSize(bf, 8);
            templateNumPage.SetTextMatrix(0, 0);
            templateNumPage.ShowText("" + (writer.PageNumber - 1));
            templateNumPage.EndText();
        } 

    }
}
