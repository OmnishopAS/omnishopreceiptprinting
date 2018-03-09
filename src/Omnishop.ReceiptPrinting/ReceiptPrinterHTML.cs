using System;
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
        readonly RDLFont _writtenFont = new RDLFont();
        readonly RDLFont _currentFont = new RDLFont();
        readonly RDLLineSettings _writtenLineSettings = new RDLLineSettings();
        readonly RDLLineSettings _currentLineSettings = new RDLLineSettings();

        readonly StringBuilder _sb = new StringBuilder();

        public RDLFont Font => _currentFont;

        public RDLLineSettings LineSettings => _currentLineSettings;

        public void WriteText(string text, bool newLineAfter = true)
        {
            //WriteFontAndLineSettingsIfChanged();
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

        public void WriteBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        public void WriteInitPage()
        {
            _sb.AppendLine(@"<pre><div style=""display: block; font-family: monospace; white-space: pre; margin:1em 0;"">");
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

        private void WriteFontAndLineSettingsIfChanged()
        {
            if (!_writtenFont.IsEqual(_currentFont))
            {
                WriteFontStyle(_currentFont);
                _currentFont.CloneToOther(_writtenFont);
            }

            if (!_writtenLineSettings.IsEqual(_currentLineSettings))
            {
                WriteLineSettings(_currentLineSettings);
                _currentLineSettings.CloneToOther(_writtenLineSettings);
            }
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

        private void WriteFontStyle(RDLFont font)
        {
            //var styleByte = (byte)font.FontType;
            //if (font.Bold)
            //    styleByte += 8;
            //if (font.DoubleHeight)
            //    styleByte += 16;
            //if (font.DoubleWidth)
            //    styleByte += 32;

            //_sb.Append(ESC + "!" + (char)styleByte);

            //if (font.Underline)
            //    _sb.Append(ESC + "-" + (char)01);
            //else
            //    _sb.Append(ESC + "-" + (char)00);
        }

        private void WriteLineSettings(RDLLineSettings lineSettings)
        {
            var alignmentValue = (byte)lineSettings.Alignment;
            //_sb.Append(ESC + "a" + alignmentValue);
        }

    }

}
