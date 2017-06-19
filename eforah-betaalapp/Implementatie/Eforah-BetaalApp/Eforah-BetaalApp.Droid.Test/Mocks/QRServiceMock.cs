using Eforah_BetaalApp.Implementation.Services;
using System;
using System.Collections.Generic;

namespace Eforah_BetaalApp.Droid.Test.Mocks
{
    class QRServiceMock : IQRService
    {
        byte[] IQRService.GenerateQRCode(string data, int w, int h, int m)
        {
            List<byte> AllData = new List<byte>();
            byte b = 0xFA ;

            for (int i = 0; i < w*h; i++)
            {
                AllData.Add(b);
                AllData.Add(b);
                AllData.Add(b);
                AllData.Add(b);
            }

            //No casting required
            byte[] bytearray = AllData.ToArray();

            return bytearray;
        }
    }
}