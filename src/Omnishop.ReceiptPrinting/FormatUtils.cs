﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Omnishop.ReceiptPrinting
{
    public static class FormatUtils
    {
        public static string CutOrPadLeft(string value, int totalLength)
        {
            if (string.IsNullOrEmpty(value))
                return new string(' ', totalLength);

            if (value.Length >= totalLength)
                return value.Substring(0, totalLength);

            return value.PadLeft(totalLength);
        }

        public static string CutOrPadRight(string value, int totalLength)
        {
            if (value == null)
                return new string(' ', totalLength);

            if (value.Length >= totalLength)
                return value.Substring(0, totalLength);

            return value.PadRight(totalLength, ' ');
        }

    }
}
