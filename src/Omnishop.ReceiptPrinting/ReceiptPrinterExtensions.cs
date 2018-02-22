using System;
using System.Collections.Generic;
using System.Text;

namespace Omnishop.ReceiptPrinting
{
    public static class ReceiptPrinterExtensions
    {
        public static void WriteSeparatorLine(this IReceiptPrinter receiptPrinter, int lineLength=42)
        {
            var oldBold = receiptPrinter.Font.Bold;
            var oldUnderline = receiptPrinter.Font.Underline;
            receiptPrinter.Font.Bold = true;
            receiptPrinter.Font.Underline = true;
            receiptPrinter.WriteText(new string(' ', lineLength));
            receiptPrinter.Font.Bold = false;
            receiptPrinter.Font.Underline = false;
            receiptPrinter.WriteText("");
            receiptPrinter.Font.Bold = oldBold;
            receiptPrinter.Font.Underline = oldUnderline;
        }
    }
}
