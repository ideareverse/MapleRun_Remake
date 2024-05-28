using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CommonModule.org.io
{
    public class BitmapToBitmapSource
    {
        private Bitmap bitmap = null;
        public BitmapToBitmapSource(Bitmap bitmap) { 
            this.bitmap = bitmap;
        }
        public BitmapSource getBitmapSource()
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            bitmap.GetHbitmap(),
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions()
            );
            return bitmapSource;

        }
    }
}
