using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing;

namespace Eforah_BetaalApp.Implementation.Test.Mocks
{
    class BarcodeWriterMock : ZXing.BarcodeWriter
    {
        public ZXing.Common.EncodingOptions options;
        public BarcodeFormat format;

        public new byte[] Write(String data)
        {
            if(options == null || format != BarcodeFormat.QR_CODE)
            {
                return null;
            }

            byte[] b = Encoding.ASCII.GetBytes(data); //{ 0xff, 0xfa, 0xf0, 0xaf, 0x0f, 0x00 };
            return b;
        }
    }
}