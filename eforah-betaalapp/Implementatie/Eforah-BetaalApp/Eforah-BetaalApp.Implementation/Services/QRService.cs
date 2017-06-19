using System;
using ZXing;

namespace Eforah_BetaalApp.Implementation.Services
{
    public class QRService : IQRService
    {
        BarcodeWriter writer;

        public QRService()
        {
            writer = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
        }

        public QRService(BarcodeWriter pWriter) : this()
        {
            if (pWriter != null)
            {
                writer = pWriter;
            }

            writer.Format = BarcodeFormat.QR_CODE;
        }

        /// <summary>
        /// Generates a QR Code of the given data and converts the Data to a Bitmap.
        /// </summary>
        /// <param name="Data">The data to be encoded into a QR Code</param>
        /// <param name="width">The desired width of the image</param>
        /// <param name="height">The desired height of the image</param>
        /// <param name="margin">The desired margin of the image</param>
        /// <returns>A Byte[] of the Pixels (Alpha and RGB color values)</returns>
        public Byte[] GenerateQRCode(string Data, int width, int height, int margin)
        {
            if (Data == null || Data.Length <= 0) { throw new System.ArgumentException("Parameter cannot be null or empty.", "Data"); }
            if (width <= 0) { throw new ArgumentException("Width can't be smaller than 1.", "width"); }
            if (height <= 0) { throw new ArgumentException("Height can't be smaller than 1.", "height"); }

            if (margin < 0) { margin = 0; }

            writer.Options = new ZXing.Common.EncodingOptions
            {
                Height = height,
                Width = width,
                Margin = margin
            };

            var bytes = writer.Write(Data);
            return bytes;
        }
    }
}
