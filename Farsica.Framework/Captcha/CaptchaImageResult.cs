namespace Farsica.Framework.Captcha
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class CaptchaImageResult : ActionResult
    {
        // عرض تصویر
        private const int Width = 250;

        // طول تصویر
        private const int Height = 50;

        // نوع فونت
        private const string CaptchaFontFamily = "Tahoma";

        // اندازه سایز فونت
        private const int CaptchaFontSize = 16;

        private static readonly Color[] TextColors =
        {
            Color.Black,
            Color.Blue,
            Color.Brown,
            Color.DarkCyan,
            Color.DarkGray,
            Color.DarkGreen,
            Color.Green,
            Color.DarkKhaki,
            Color.DarkGoldenrod,
            Color.DarkMagenta,
            Color.DarkRed,
            Color.DarkSlateBlue,
            Color.DarkTurquoise,
        };

        // رنگ پس زمینه
        private static readonly Color BackgroundColor = Color.FromArgb(255, 255, 255, 255);

        private enum Operator : byte
        {
            Plus = 0,
            Minuse = 1,
            MultipleBy = 2,
            DividedBy = 3,
        }

        public static string CreateCaptcha(HttpResponse response)
        {
            // ایجاد دو عدد تصادفی
            var randomNumber1 = 0;
            var randomNumber2 = 0;
            var resultNumber = 0;

            var operation = (Operator)new Random().Next(0, 3);
            switch (operation)
            {
                case Operator.Plus:
                    randomNumber1 = CreateRandomNumber(2);
                    randomNumber2 = CreateRandomNumber(1);
                    resultNumber = randomNumber1 + randomNumber2;
                    break;
                case Operator.Minuse:
                    randomNumber1 = CreateRandomNumber(2);
                    randomNumber2 = CreateRandomNumber(1);
                    resultNumber = randomNumber1 - randomNumber2;
                    break;
                case Operator.MultipleBy:
                    randomNumber1 = CreateRandomNumber(1);
                    randomNumber2 = CreateRandomNumber(1);
                    resultNumber = randomNumber1 * randomNumber2;
                    break;
                case Operator.DividedBy:
                    var selectRandomNumber = true;
                    randomNumber1 = CreateRandomNumber(2);
                    while (selectRandomNumber)
                    {
                        randomNumber2 = CreateRandomNumber(1);
                        if (randomNumber1 % randomNumber2 == 0)
                        {
                            selectRandomNumber = false;
                        }
                    }

                    resultNumber = randomNumber1 / randomNumber2;
                    break;
            }

            // تنظیمات فرمت متن تصویر امنیتی
            var format = new StringFormat
            {
                FormatFlags = StringFormatFlags.DirectionRightToLeft,
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };

            // نوع و اندازه قلم تصویر امنیتی
            using var font = new Font(CaptchaFontFamily, CaptchaFontSize, FontStyle.Bold | FontStyle.Italic);
            var randomString = RandomString(6);

            //-- ایجاد یک شیء گرافیکی برای عملیات ترسیم روی تصویر امنیتی
            using var bitmap = new Bitmap(Width, Height);
            using (var gfxCaptchaImage = Graphics.FromImage(bitmap))
            {
                //-- پاک کردن پس زمینه تصویر امنیتی با یک رنگ سفید
                gfxCaptchaImage.Clear(BackgroundColor);

                // ایجادمتن در تصویر امنیتی
                var random = new Random();
                var solidBrush = new SolidBrush(TextColors[random.Next(13)]);

                // تبدیل عدد اتفاقی تولید شده به حروف معادل
                // var randomString = $"{NumberToStringConverter.Convert(randomNumber1)} {GlobalResource.ResourceManager.GetString(operation.ToString())} {NumberToStringConverter.Convert(randomNumber2)}";
                gfxCaptchaImage.DrawString(randomString, font, solidBrush, new Rectangle(0, 0, Width, Height), format);

                // ایجاد رنگ متن تصویر امنیتی به صورت اتفاقی و مسیر گرافیکی
                var pen = new Pen(Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100)));
                gfxCaptchaImage.DrawPath(pen, new GraphicsPath());

                // اضافه کردن نویز به تصویر امنیتی
                int i, r, xx, yy, u, v;
                for (i = 1; i < 10; i++)
                {
                    pen.Color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    r = random.Next(0, Width / 3);
                    xx = random.Next(0, Width);
                    yy = random.Next(0, Height);
                    u = xx - r;
                    v = yy - r;
                    gfxCaptchaImage.DrawEllipse(pen, u, v, r, r);
                }

                // رسم تصویر امنیتی
                gfxCaptchaImage.DrawImage(bitmap, new Point(0, 0));
                gfxCaptchaImage.Flush();
            }

            // رمزنگاری مقدار متغیر بالا جهت ذخیره در کوکی
            var encryptedValue = SecurityExtensions.Encrypt(randomString);
            var data = $"{DateTime.UtcNow.Ticks}|{encryptedValue}";

            response.Cookies.Append(SecurityExtensions.Captcha, data, new CookieOptions { Expires = DateTime.Now.AddMinutes(10), });
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "text";
            await response.Body.WriteAsync(Encoding.UTF8.GetBytes(CreateCaptcha(response)));
        }

        private static string? RandomString(int length)
        {
            var random = new Random();
            const string pool = "0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        private static int CreateRandomNumber(int digitNumber)
        {
            var random = new Random();
            switch (digitNumber)
            {
                case 1:
                    return random.Next(1, 9);
                case 2:
                    return random.Next(10, 99);
                case 3:
                    return random.Next(100, 999);
                default:
                    return random.Next(1000, 9999);
            }
        }
    }
}
