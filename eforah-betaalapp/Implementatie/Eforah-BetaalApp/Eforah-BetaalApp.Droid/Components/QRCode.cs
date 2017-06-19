using System;
using Android.App;
using Android.Graphics;
using ZXing.Mobile;
using Eforah_BetaalApp.Implementation.Services;

namespace Eforah_BetaalApp.Droid.Components
{
    public class QRCode
    {
        int margin = 1;
        IQRService QRService;

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app">Application needed to initialize the MobileBarcodeScanner</param>
        public QRCode(Application app)
        {
            //Initializing the barcode scanner used to generate the QR code
            MobileBarcodeScanner.Initialize(app);

            //Create QR Generator
            QRService = new QRService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app">Application needed to initialize the MobileBarcodeScanner</param>
        /// <param name="pGenerator">The QR code generator to use.</param>
        public QRCode(Application app, IQRService pGenerator) : this(app)
        {
            if (pGenerator != null)
            {
                QRService = pGenerator;
            }
        }
        #endregion

        /// <summary>
        /// Generates a QR Code of the given data and converts the Data to a Bitmap.
        /// </summary>
        /// <param name="Data">The data to be encoded into a QR Code</param>
        /// <param name="width">The desired width of the image</param>
        /// <param name="height">The desired height of the image</param>
        /// <returns>A Bitmap image of the QR Code</returns>
        public Bitmap GenerateQRCode(String Data, int width, int height)
        {
            if (Data == null || Data.Length <= 0) { throw new System.ArgumentException("Parameter cannot be null or empty.", "Data"); }
            if (width <= 0) { throw new System.ArgumentException("Width can't be smaller than 1.", "width"); }
            if (height <= 0) { throw new System.ArgumentException("Height can't be smaller than 1.", "height"); }

            //Generate QR and get it as Array of Bytes
            Byte[] v = QRService.GenerateQRCode(Data, width, height, margin);

            //Array of the pixels represented as ints (condenses 4 bytes (alpha, red, green, blue))
            int[] i = new int[v.Length / 4];

            //iterator
            int k = 0;

            //4 Bytes become an int (alpha, red, green, blue)
            for (int j = 0; j < v.Length; j += 4)
            {
                Byte[] b = { 255, v[j + 1], v[j + 2], v[j + 3] };
                i[k] = BitConverter.ToInt32(b, 0);
                k++;
            }

            //Create Bitmap of a size from pixels.
            Bitmap bm = Bitmap.CreateBitmap(i, width, height, Bitmap.Config.Argb8888);

            return bm;
        }
    }
}