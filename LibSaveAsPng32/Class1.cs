using LibSaveAsPng32.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSaveAsPng32
{
    [ComVisible(true)]
    [Guid("cc2d1ec3-f115-3d64-9979-c1d9b4dbd5fc")]
    [ProgId("LibSaveAsPng32.Class1")]
    public class Class1 : Class1Interface
    {
        public void SavePictureAs(object picture, string saveTo)
        {
            BitmapUtil.To32bppBitmap(((dynamic)picture).Handle).Save(saveTo);
        }

    }
}
