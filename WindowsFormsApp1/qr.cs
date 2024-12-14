using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;
using Zen.Barcode;





namespace WindowsFormsApp1
{
    public partial class qr : Form
    {
        private string tenphim;
        private string thoigian;
        private string tenphong;
        private PrintDocument printDoc;
        private string textToPrint;
        public qr()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
        }
        public qr(string tenP, string tg, string phong)
        {
            InitializeComponent();
            this.tenphim = tenP;
            this.thoigian = tg;
            this.tenphong = phong;
            this.FormBorderStyle = FormBorderStyle.None;
            printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
        }
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font printFont = new Font("Arial", 12); // Font cho n?i dung van b?n
            float leftMargin = e.MarginBounds.Left; // L? trái
            float topMargin = e.MarginBounds.Top;  // L? trên

            // Kích thu?c logo
            int logoWidth = 50;
            int logoHeight = 50;

            try
            {
                // L?y logo t? Resources
                Image logo = Properties.Resources.logo;

                // Làm m? logo b?ng cách s? d?ng ImageAttributes
                ImageAttributes imageAttr = new ImageAttributes();
                ColorMatrix colorMatrix = new ColorMatrix
                {
                    Matrix33 = 0.3f // Ð? trong su?t (0.0 - hoàn toàn trong su?t, 1.0 - không trong su?t)
                };
                imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // V? logo l?p l?i trên toàn vùng in (làm n?n)
                for (float y = 0; y < e.PageBounds.Height; y += logoHeight + 10)
                {
                    for (float x = 0; x < e.PageBounds.Width; x += logoWidth + 10)
                    {
                        e.Graphics.DrawImage(
                            logo,
                            new Rectangle((int)x, (int)y, logoWidth, logoHeight), // V? trí và kích thu?c logo
                            0, 0, logo.Width, logo.Height, // Toàn b? logo g?c
                            GraphicsUnit.Pixel,
                            imageAttr // Áp d?ng hi?u ?ng làm m?
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không th? t?i logo: " + ex.Message);
            }

            // V? n?i dung lên trên logo
            string text = "======= Ve Xem Phim ==========\nPhim: Avengers: Endgame \nThoi Gian Chieu: 7:00 PM \nPhong Chieu: Phòng 1";
            e.Graphics.DrawString(text, printFont, Brushes.Black, leftMargin, topMargin);

            // T?o mã QR và v? vào trang in
            Bitmap qrImage = GenerateQRCode("Phim: Avengers: Endgame, Thoi Gian Chieu: 7:00 PM, Phong Chieu: Phòng 1");
            float qrTop = topMargin + 100; // V? trí mã QR
            float qrHeight = 150; // Chi?u cao mã QR
            float qrWidth = 150; // Chi?u r?ng mã QR
            e.Graphics.DrawImage(qrImage, leftMargin, qrTop, qrWidth, qrHeight);
        }

        private Bitmap GenerateQRCode(string content)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrImage = qrCode.GetGraphic(2); // Kích thu?c mã QR
            return qrImage;
        }






        private void qr_Load(object sender, EventArgs e)
        {

            QR();

        }

        public void QR()
        {
            string qrConten = string.Format(
    "========= Ve Xem Phim ========= \n" +
    " {0,-24} : {1,-32}\n" +
    " {2,-13} : {3,-34}\n" +
    " {4,-16} : {5,-30}\n" +
    "==============================",
    "Phim", tenphim, "Thoi Gian Chieu", thoigian, "Phong Chieu", tenphong);
            using (QRCodeGenerator qr = new QRCodeGenerator())
            {
                QRCodeData data = qr.CreateQrCode(qrConten, QRCodeGenerator.ECCLevel.Q);
                using (QRCode code = new QRCode(data))
                {
                    Bitmap qrimg = code.GetGraphic(2, Color.Red, Color.White, true);
                    pictureBox1.Image = qrimg;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // N?i dung c?n in (có th? là n?i dung QR code)
            textToPrint = "======= Ve Xem Phim ==========\n" +
                          "Phim : Avengers: Endgame \n" +
                          "Thoi Gian Chieu : 7:00 PM \n" +
                          "Phong Chieu : Phòng 1 \n" +
                          "===============================";

            // T?o mã QR t? d? li?u
            string qrContent = "Phim: Avengers: Endgame, Thoi Gian Chieu: 7:00 PM, Phong Chieu: Phòng 1";
            Bitmap qrImage = GenerateQRCode(qrContent);
            // M? h?p tho?i ch?n máy in và in
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }
    }
}
