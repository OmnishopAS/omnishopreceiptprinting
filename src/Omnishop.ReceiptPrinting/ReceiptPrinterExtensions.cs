using System;
using System.Collections.Generic;
using System.Text;

namespace Omnishop.ReceiptPrinting
{
    public static class ReceiptPrinterExtensions
    {
        public static void WriteSeparatorLine(this IReceiptPrinter receiptPrinter)
        {
            var oldBold = receiptPrinter.Font.Bold;
            var oldUnderline = receiptPrinter.Font.Underline;
            receiptPrinter.Font.Bold = true;
            receiptPrinter.Font.Underline = true;
            receiptPrinter.WriteText(new string(' ', 56));
            receiptPrinter.Font.Bold = false;
            receiptPrinter.Font.Underline = false;
            receiptPrinter.WriteText("");
            receiptPrinter.Font.Bold = oldBold;
            receiptPrinter.Font.Underline = oldUnderline;
        }
    }
}
