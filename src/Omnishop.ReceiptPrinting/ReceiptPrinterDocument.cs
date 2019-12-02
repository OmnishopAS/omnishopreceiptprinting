using System.Collections.Generic;

namespace Omnishop.ReceiptPrinting
{
    public class ReceiptPrinterDocument : IReceiptPrinter
    {
        readonly RDLFont _writtenFont = new RDLFont();
        readonly RDLFont _currentFont = new RDLFont();
        readonly RDLLineSettings _writtenLineSettings= new RDLLineSettings();
        readonly RDLLineSettings _currentLineSettings = new RDLLineSettings();

        readonly List<ReceiptDocumentLine> _writtenLines = new List<ReceiptDocumentLine>();

        public RDLFont Font => _currentFont;

        public RDLLineSettings LineSettings => _currentLineSettings;


        public void WriteText(string text, bool newLineAfter = true)
        {
            WriteFontAndLineSettingsIfChanged();
            var line = new RDLText()
            {
                Text = text,
                NewLineAfter = newLineAfter
            };
            _writtenLines.Add(line);
        }

        public void WriteBarcode(string barcode, BarcodeTypes barcodeType = BarcodeTypes.Default)
        {
            var line = new RDLBarcode()
            {
                Barcode = barcode,
                Type=barcodeType
            };
            _writtenLines.Add(line);
        }

        private void WriteFontAndLineSettingsIfChanged()
        {
            if (!_writtenFont.IsEqual(_currentFont))
            {
                var newFont = new RDLFont();
                _currentFont.CloneToOther(newFont);
                _writtenLines.Add(newFont);
                _currentFont.CloneToOther(_writtenFont);
            }

            if(!_writtenLineSettings.IsEqual(_currentLineSettings))
            {
                var newSettings = new RDLLineSettings();
                _currentLineSettings.CloneToOther(newSettings);
                _writtenLines.Add(newSettings);
                _currentLineSettings.CloneToOther(_writtenLineSettings);
            }
        }

        public ReceiptDocument GetReceiptDocument()
        {
            return new ReceiptDocument()
            {
                ReceiptLines = _writtenLines
            };
        }
    }

}
