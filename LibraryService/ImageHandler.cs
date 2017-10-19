using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Library
{
    public class ImageHandler
    {
        public Bitmap ImgSource { get; }

        private Size NewImageSize(Image fullSizeImage, int resize)
        {
            int newHeight;
            int newWidth;

            if (fullSizeImage.Width > fullSizeImage.Height)
            {
                newWidth = fullSizeImage.Width * resize / fullSizeImage.Height;
                newHeight = resize;
            }
            else
            {
                newHeight = fullSizeImage.Height * resize / fullSizeImage.Width;
                newWidth = resize;
            }

            return new Size(newWidth, newHeight);
        }

        private Bitmap ResizeImage(Image image, int resize)
        {

            Size imageSize = NewImageSize(image, resize);
            Rectangle destRect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
            Bitmap destImage = new Bitmap(imageSize.Width, imageSize.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public ImageHandler(Stream file, int size)
        {
            ImgSource = ResizeImage(Image.FromStream(file), size);
        }
    }
}
