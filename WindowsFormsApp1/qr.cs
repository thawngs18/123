using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            Font printFont = new Font("Arial", 12); // Định dạng font cho văn bản
            float leftMargin = e.MarginBounds.Left; // Lấy vị trí lề trái
            float topMargin = e.MarginBounds.Top; // Lấy vị trí lề trên

            // Vẽ chuỗi văn bản vào trang in
            e.Graphics.DrawString(textToPrint, printFont, Brushes.Black, leftMargin, topMargin);

            // Tạo mã QR và vẽ vào trang in
            Bitmap qrImage = GenerateQRCode("Phim: Avengers: Endgame, Thoi Gian Chieu: 7:00 PM, Phong Chieu: Phòng 1");
            e.Graphics.DrawImage(qrImage, leftMargin, topMargin + 100); // Vẽ mã QR cách văn bản một khoảng
        }

        private Bitmap GenerateQRCode(string content)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrImage = qrCode.GetGraphic(2); // Kích thước mã QR
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
                    Bitmap qrimg = code.GetGraphic(2,Color.Red,Color.White,true);
                    pictureBox1.Image = qrimg;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Nội dung cần in (có thể là nội dung QR code)
            textToPrint = "======= Ve Xem Phim ==========\n" +
                          "Phim : Avengers: Endgame \n" +
                          "Thoi Gian Chieu : 7:00 PM \n" +
                          "Phong Chieu : Phòng 1 \n" +
                          "===============================";

            // Tạo mã QR từ dữ liệu
            string qrContent = "Phim: Avengers: Endgame, Thoi Gian Chieu: 7:00 PM, Phong Chieu: Phòng 1";
            Bitmap qrImage = GenerateQRCode(qrContent);

            // Mở hộp thoại chọn máy in và in
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }
    }
}
