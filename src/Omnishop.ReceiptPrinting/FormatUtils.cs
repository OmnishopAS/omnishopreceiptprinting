using System;
using System.Collections.Generic;
using System.Text;

namespace Omnishop.ReceiptPrinting
{
    public static class FormatUtils
    {
        public static string CutOrPadLeft(this string value, int totalLength)
        {
            if (string.IsNullOrEmpty(value))
                return new string(' ', totalLength);

            if (value.Length >= totalLength)
                return value.Substring(0, totalLength);

            return value.PadLeft(totalLength);
        }

        public static string CutOrPadRight(this string value, int totalLength)
        {
            if (string.IsNullOrEmpty(value))
                return new string(' ', totalLength);

            if (value.Length >= totalLength)
                return value.Substring(0, totalLength);

            return value.PadRight(totalLength);
        }
    }
}
