using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSaveAsPng32.Utils
{
    static class BitmapUtil
    {
        internal static Bitmap To32bppBitmap(int bitmapHandle)
        {
            var dibsection = new DIBSECTION();

            if (GetObjectDIBSection((IntPtr)bitmapHandle, Marshal.SizeOf(dibsection), ref dibsection) == 0)
            {
                throw new ArgumentException("The taken picture isn't a valid bitmap suitable for getting DIBSECTION.");
            }

            if (dibsection.dsBm.bmBitsPixel != 32)
            {
                throw new ArgumentException("The taken picture isn't 32-bpp bitmap.");
            }

            var w = dibsection.dsBm.bmWidth;
            var h = dibsection.dsBm.bmHeight;

            // Create the destination Bitmap object.
            var image = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            // Gets a pointer to the raw bits.
            var pBits = new byte[4 * w * h];

            Marshal.Copy(dibsection.dsBm.bmBits, pBits, 0, pBits.Length);

            // Scan the image to check if alpha channel is empty
            // 24bpp RGB when false, 32bpp ARGB when true.
            bool alpha = false;
            for (int x = 0; x < dibsection.dsBmih.biWidth; x++)
            {
                for (int y = 0; y < dibsection.dsBmih.biHeight; y++)
                {
                    alpha |= pBits[4 * (y * dibsection.dsBmih.biWidth + x) + 3] != 0;
                }
            }

            // Copy each pixel byte for byte.
            for (int x = 0; x < dibsection.dsBmih.biWidth; x++)
            {
                for (int y = 0; y < dibsection.dsBmih.biHeight; y++)
                {
                    var offset = 4 * (y * dibsection.dsBmih.biWidth + x);
                    if (pBits[offset + 3] != 0)
                    {
                        // having alpha, keep transparency
                    }
                    else if (!alpha)
                    {
                        // no alpha, it is an opaque
                        pBits[offset + 3] = 255;
                    }
                }
            }

            var bitmapData = image.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                Marshal.Copy(pBits, 0, bitmapData.Scan0, pBits.Length);
            }
            finally
            {
                image.UnlockBits(bitmapData);
            }

            return image;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAP
        {
            public Int32 bmType;
            public Int32 bmWidth;
            public Int32 bmHeight;
            public Int32 bmWidthBytes;
            public Int16 bmPlanes;
            public Int16 bmBitsPixel;
            public IntPtr bmBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int bitClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DIBSECTION
        {
            public BITMAP dsBm;
            public BITMAPINFOHEADER dsBmih;
            public int dsBitField1;
            public int dsBitField2;
            public int dsBitField3;
            public IntPtr dshSection;
            public int dsOffset;
        }

        [DllImport("gdi32.dll", EntryPoint = "GetObject")]
        private static extern int GetObjectDIBSection(IntPtr hObject, int nCount, ref DIBSECTION lpObject);
    }
}
