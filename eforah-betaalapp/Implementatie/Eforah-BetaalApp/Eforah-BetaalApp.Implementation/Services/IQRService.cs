using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eforah_BetaalApp.Implementation.Services
{
    public interface IQRService
    {
        /// <summary>
        /// Generates a QR Code of the given data and converts the Data to a Bitmap.
        /// </summary>
        /// <param name="data">The data to be encoded into a QR Code</param>
        /// <param name="w">The desired width of the image</param>
        /// <param name="h">The desired height of the image</param>
        /// <param name="m">The desired margin of the image</param>
        /// <returns>A Byte[] of the Pixels (Alpha and RGB color values)</returns>
        Byte[] GenerateQRCode(string data, int w, int h, int m);
    }
}
