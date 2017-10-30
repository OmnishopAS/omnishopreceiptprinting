using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Omnishop.ReceiptPrinting
{
    public class ReceiptDocument
    {
        [JsonProperty(ItemConverterType = typeof(ReceiptDocumentJsonConverter))]
        public List<ReceiptDocumentLine> ReceiptLines { get; set; }

        public void PrintDocument(IReceiptPrinter printer)
        {
            foreach(var line in ReceiptLines)
            {
                if(line is RDLFont font)
                {
                    font.CloneToOther(printer.Font);
                }
                else if( line is RDLLineSettings lineSettings)
                {
                    lineSettings.CloneToOther(printer.LineSettings);
                }
                else if (line is RDLText text)
                {
                    printer.WriteText(text.Text, text.NewLineAfter);
                }
                //else if (line is RDLBarcode barCode)
                //{
                //    printer.WriteBarcode(barcode.barcode);
                //}
                else
                {
                    throw new InvalidOperationException("Unkown RDL element: " + line.GetType());
                }
            }
        }
    }

    public abstract class ReceiptDocumentLine
    {

    }

    public class RDLFont : ReceiptDocumentLine
    {
        public FontTypes FontType { get; set; }
        public bool Bold { get; set; }
        public bool Underline { get; set; }
        public bool DoubleHeight { get; set; }
        public bool DoubleWidth { get; set; }

        public void CloneToOther(RDLFont other)
        {
            other.Bold = this.Bold;
            other.DoubleHeight = this.DoubleHeight;
            other.DoubleWidth = this.DoubleWidth;
            other.FontType = this.FontType;
            other.Underline = this.Underline;
        }

        public bool IsEqual(RDLFont other)
        {
            return
                other.Bold == this.Bold &&
                other.DoubleHeight == this.DoubleHeight &&
                other.DoubleWidth == this.DoubleWidth &&
                other.FontType == this.FontType &&
                other.Underline == this.Underline;
        }
    }

    public class RDLLineSettings : ReceiptDocumentLine
    {
        public TextAlignments Alignment { get; set; }

        public void CloneToOther(RDLLineSettings other)
        {
            other.Alignment = this.Alignment;
        }

        public bool IsEqual(RDLLineSettings other)
        {
            return
                other.Alignment == this.Alignment;
        }

    }

    public class RDLText : ReceiptDocumentLine
    {
        public string Text { get; set; }
        public bool NewLineAfter { get; set; }
    }

    public enum TextAlignments
    {
        Left = 0,
        Centered = 1,
        Right =2
    }

    public enum FontTypes
    {
        FontA = 0,
        FontB = 1
    }
}

