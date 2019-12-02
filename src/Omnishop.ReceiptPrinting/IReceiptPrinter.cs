namespace Omnishop.ReceiptPrinting
{
    public interface IReceiptPrinter
    {
        RDLFont Font { get; }
        RDLLineSettings LineSettings { get; }

        void WriteText(string text, bool newLineAfter = true);
        void WriteBarcode(string barcode, BarcodeTypes barcodeType=BarcodeTypes.Default);
    }
}