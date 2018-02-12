using System;
using System.Text;

namespace Omnishop.ReceiptPrinting
{
    /// <summary>
    /// IReceiptPrinter that builds a string formatted with ESC-POS for TMT88 printer
    /// InitPage should be called before any data is written.
    /// EndPage should be called when all data has been written. 
    /// </summary>
    public class ReceiptPrinterEpsonTMT88 : IReceiptPrinter
    {
        const char ESC = (char)27;           //\x1B

        readonly RDLFont _writtenFont = new RDLFont();
        readonly RDLFont _currentFont = new RDLFont();
        readonly RDLLineSettings _writtenLineSettings = new RDLLineSettings();
        readonly RDLLineSettings _currentLineSettings = new RDLLineSettings();

        readonly StringBuilder _sb = new StringBuilder();

        public RDLFont Font => _currentFont;

        public RDLLineSettings LineSettings => _currentLineSettings;

        public void WriteText(string text, bool newLineAfter = true)
        {
            WriteFontAndLineSettingsIfChanged();
            if (newLineAfter)
                _sb.AppendLine(text);
            else
                _sb.Append(text);
        }

        public void WriteBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        public void WriteInitPage()
        {
            //Init, Norsk tegnsett, Nordisk codepage (865)
            _sb.Append((char)27 + "@" + (char)27 + "R" + (char)9 + (char)27 + "t" + (char)5);

            //Skriver ut logo nr 1 i printerens NVRAM
            _sb.AppendLine("" + (char)28 + (char)112 + (char)1 + (char)0);
        }

        /// <summary>
        /// Instructs printer to flush buffer and cut receipt.
        /// </summary>
        public void WriteEndPage()
        {
            var data = (char)29 + "V" + (char)66 + (char)60; 
            _sb.AppendLine(data);
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

        private void WriteFontStyle(RDLFont font)
        {
            var styleByte = (byte)font.FontType;
            if (font.Bold)
                styleByte += 8;
            if (font.DoubleHeight)
                styleByte += 16;
            if (font.DoubleWidth)
                styleByte += 32;

            _sb.Append(ESC + "!" + (char)styleByte);

            if (font.Underline)
                _sb.Append(ESC + "-" + (char)01);
            else
                _sb.Append(ESC + "-" + (char)00);
        }

        private void WriteLineSettings(RDLLineSettings lineSettings)
        {
            var alignmentValue = (byte)lineSettings.Alignment;
            _sb.Append(ESC + "a" + alignmentValue);
        }

    }

}
