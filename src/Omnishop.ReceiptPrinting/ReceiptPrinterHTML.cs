using System;
using System.Drawing;
using System.Text;

namespace Omnishop.ReceiptPrinting
{
    /// <summary>
    /// IReceiptPrinter that builds a HTML string
    /// InitPage should be called before any data is written.
    /// EndPage should be called when all data has been written. 
    /// </summary>
    public class ReceiptPrinterHTML : IReceiptPrinter
    {
        readonly RDLFont _currentFont = new RDLFont();
        readonly RDLLineSettings _currentLineSettings = new RDLLineSettings();
        readonly StringBuilder _sb = new StringBuilder();

        public RDLFont Font => _currentFont;

        public RDLLineSettings LineSettings => _currentLineSettings;

        public void WriteText(string text, bool newLineAfter = true)
        {
            string cssClasses = GetCSSClasses();

            if (!string.IsNullOrEmpty(text))
            {
                if (newLineAfter)
                    _sb.Append("<div" + cssClasses + ">" + text + "</div>");
                else
                    _sb.Append("<span" + cssClasses + ">" + text + "</span>");
            }
            else if (newLineAfter)
                _sb.AppendLine("<br/>");
        }

        public void WriteBarcode(string barcode, BarcodeTypes barcodeType = BarcodeTypes.Default)
        {
            _sb.AppendLine(@"<img class='Barcode' src='" + this.CreateBarcodeImgSrc(barcode) + "'></img>");

        }

        private string CreateBarcodeImgSrc(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return string.Empty;

            using (var b = new BarcodeStandard.Barcode())
            {
                using (var img = b.Encode(BarcodeStandard.Type.Code39Extended, barcode, 350, 50))
                {
                    using (var skData = img.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100))
                    {
                        return @"data:image/png;base64," + Convert.ToBase64String(skData.AsSpan().ToArray());
                    }                   
                }
            }
        }

        public void WriteInitPage()
        {
            _sb.AppendLine(@"<pre class=""receipt""><div style=""display: block; font-family: monospace; white-space: pre; margin:1em 0;"">");
        }

        /// <summary>
        /// Instructs printer to flush buffer and cut receipt.
        /// </summary>
        public void WriteEndPage()
        {
            _sb.AppendLine("</div></pre>");
        }

        public string GetWrittenString()
        {
            return _sb.ToString();
        }

        private string GetCSSClasses()
        {
            string cssClasses = "";

            if (_currentFont.Bold)
                cssClasses += "receipt_bold ";
            if (_currentFont.DoubleHeight && _currentFont.DoubleWidth)
                cssClasses += "receipt_large ";
            if (_currentFont.Underline)
                cssClasses += "receipt_underlined ";
            if (_currentLineSettings.Alignment == TextAlignments.Centered)
                cssClasses += "receipt_centered ";
            else if (_currentLineSettings.Alignment == TextAlignments.Right)
                cssClasses += "receipt_rightaligned ";

            if (!string.IsNullOrEmpty(cssClasses))
                cssClasses = " class=\"" + cssClasses.TrimEnd(' ') + "\"";
            return cssClasses;
        }

    }

}
