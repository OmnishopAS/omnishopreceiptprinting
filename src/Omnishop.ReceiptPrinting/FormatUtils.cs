using System;
using System.Collections.Generic;
using System.Linq;
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
            if (value == null)
                return new string(' ', totalLength);

            if (value.Length >= totalLength)
                return value.Substring(0, totalLength);

            return value.PadRight(totalLength, ' ');
        }

        public static IEnumerable<string> Split(string str, int chunkSize)
        {
            var retValue = new List<string>();
            if (String.IsNullOrEmpty(str))
            {
                retValue.Add(String.Empty);
            }
            else
            {
                int index = 0;
                while (index < str.Length)
                {
                    var subStringLength = Math.Min(chunkSize, str.Length - index);
                    retValue.Add(str.Substring(0, subStringLength));
                    index += subStringLength;
                }

                return retValue.Select(x => CutOrPadRight(x, chunkSize));
            }
            return retValue;

        }

    }
}
