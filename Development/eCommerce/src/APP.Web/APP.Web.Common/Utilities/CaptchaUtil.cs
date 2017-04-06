using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web;
using APP.Manager.Common.Exception;

namespace APP.Web.Common.Utilities
{
    [Serializable]
    public static class CaptchaUtil
    {
        private static readonly Font font = new Font("Georgia", 14, FontStyle.Italic | FontStyle.Bold);
        private static readonly PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
        private static readonly Color backgroundColor = Color.Black;
        private static readonly Brush brush = new SolidBrush(Color.White);
        private static readonly Random r = new Random();


        public static byte[] GetCaptchaImage(string captchaSessionKey)
        {
            string captchaText = GetRandomAlphaNumeric();
            HttpContext.Current.Session[captchaSessionKey] = captchaText;
            using (var image = GenerateImage(captchaText))
            {
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Jpeg);
                    return stream.ToArray();
                }
            }
        }

        public static bool VerifyCaptcha(string captchaKey, string captchaValue)
        {
            object captcha = HttpContext.Current.Session[captchaKey];
            if (captcha == null)
            {
                throw new AccountException("CAPTCHA EXPIRE:" + captchaKey, Entities.Enum.ErrorEnum.ErrCode.MEMBER_CAPTCHA_EXPIRE);
            }
            HttpContext.Current.Session[captchaKey] = null; // Must clear value from session
            return captchaValue.Equals(captcha);
        }

        private static string GetRandomAlphaNumeric()
        {
            return r.Next(9999).ToString("0000");
        }

        private static Bitmap GenerateImage(string sTextToImg)
        {
            Bitmap bitMap = new Bitmap(66, 22, pixelFormat);

            using (Graphics gdImageGrp = Graphics.FromImage(bitMap))
            {
                gdImageGrp.Clear(backgroundColor);
                gdImageGrp.TextRenderingHint = TextRenderingHint.AntiAlias;
                gdImageGrp.DrawString(sTextToImg, font, brush, 0, 0);
                gdImageGrp.Flush();
                return bitMap;
            }
        }
    }
}
