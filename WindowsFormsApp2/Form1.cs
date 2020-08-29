using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private Bitmap bit;
        public Form1()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image<Bgr, byte> imgInput = new Image<Bgr, byte>(open.FileName);
                    pictureBox1.Image = imgInput.Bitmap;

                    //resize gambar menjadi x,y pixel
                    Image<Bgr, Byte> resize = imgInput.Resize(400, 400, Emgu.CV.CvEnum.Inter.Cubic);
                    this.pictureBox1.Image = resize.ToBitmap();


                    //mengeget gambar atau clone gambar dari picture box
                    bit = (Bitmap)pictureBox1.Image.Clone();

                    //mengambil info lokasi gambar
                    // tb_lokasigambar.Text = open.FileName;

                    //mengambil nilai resolusi
                    int a = pictureBox1.Width;
                    int b = pictureBox2.Height;

                    //   Bitmap b = new Bitmap(pbInput.Image);
                    bit.MakeTransparent(Color.FromArgb(255, 255, 255));
                    pictureBox1.Image = bit;

                   // progressBar1.Visible = true;

               

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
        
            // pembuatan objek Bitmap dengan nama variabel bitmap
            // pbInput.Image : mengambil gambar yang ada dalam picture box
            // Bitmap adalah konversi dari picturebox ke bitmap 
            Bitmap bitmap = new Bitmap(pictureBox1.Image); //deklarasi bitmap baru
            {

                // inisialisasi variabel byte
                // range 0 - 255
                byte t0, t, pixel, result;

                // inisialisasi variabel dengan tipe integer
                // digunakan untuk menghitung rata-rata nilai yang telah dibagi
                // dalam 2 segmen
                int miu1, miu2;

                // inisialisasi variabel list untuk menampung nilai yang masuk
                // dalam segmen 1 dan segmen 2
                ArrayList g1 = new ArrayList();
                ArrayList g2 = new ArrayList();

                // T =  threshold
                // pemberian nilai awal pada t0
                // dan t yang digunakan untuk menampung nilai threshold untuk citra
                t0 = 0;
                t = 0;

                // gunakan do.. while..
                // agar fungsi berjalan setidaknya 1 kali dan baru dicek kondisinya
                // setelah fungsi dijalankan
                do
                {
                    // nilai t0 sama dengan nilai t yang telah dihitung dalam perulangan
                    t0 = t;

                    // reset nilai miu1 dan miu2 yang akan digunakan untuk perhitungan
                    miu1 = 0;
                    miu2 = 0;

                    // reset isi list dari g1 dan g2 yang akan digunakan untuk menampung nilai tiap segmen
                    g1.Clear();
                    g2.Clear();

                    // perulangan untuk mengisi segmen 1 dan segmen 2
                    // dimana bila nilai pixel di titik (x,y) > T, maka masuk dalam segmen 1
                    // dan jika nilai pixel di titik (x,y) <= T, maka masuk dalam segmen 2
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Console.WriteLine(y + " " + x);
                            pixel = bitmap.GetPixel(x, y).R;

                            if (pixel > t)
                            {
                                g1.Add(pixel);
                            }
                            else
                            {
                                g2.Add(pixel);
                            }
                        }
                    }


                    // perulangan FOREACH yang digunakan untuk
                    // looping atau perulangan untuk menjumlahkan seluruh isi dalam list g1
                    // dan menampungnya dalam variabel miu1
                    foreach (byte value in g1)
                    {
                        miu1 = miu1 + value;
                    }

                    // perulangan FOREACH yang digunakan untuk
                    // looping atau perulangan untuk menjumlahkan seluruh isi dalam list g2
                    // dan menampungnya dalam variabel miu2
                    foreach (byte value in g2)
                    {
                        miu2 = miu2 + value;
                    }

                    // kondisi IF.. ELSE.. yang digunakan untuk
                    // handle error bila isi dari list g1 = 0
                    // karena hasilnya akan tak hingga bila dibagi dengan 0
                    if (g1.Count > 0)
                    {
                        miu1 = (byte)(miu1 / g1.Count);
                    }
                    else
                    {
                        miu1 = 0;
                    }

                    // kondisi IF.. ELSE.. yang digunakan untuk
                    // handle error bila isi dari list g2 = 0
                    // karena hasilnya akan tak hingga bila dibagi dengan 0
                    if (g2.Count > 0)
                    {
                        miu2 = (byte)(miu2 / g2.Count);
                    }
                    else
                    {
                        miu2 = 0;
                    }

                    // melakukan perhitungan untuk t
                    t = (byte)((miu1 + miu2) / 2);

                    // lakukan perulangan bila t > t0
                }
                while (t > t0);

                // perulangan untuk melakukan proses threshold
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        // menampung nilai R dalam variabel pixel
                        pixel = bitmap.GetPixel(x, y).R;

                        // kondisi IF.. ELSE.. untuk menerapkan algoritma global threshold
                        // IF f(x,y) > T, THEN white (255)
                        // ELSE IF f(x,y) <= T, THEN black (0)
                        // untuk penyederhanaan perulangan, maka cukup digunakan IF.. ELSE..
                        if (pixel > t)
                        {
                            result = 255;
                        }
                        else
                        {
                            result = 0;
                        }

                        // set nilai pixel pada titik (x,y) dengan nilai baru pada variabel result
                        bitmap.SetPixel(x, y, Color.FromArgb(result, result, result));
                    }

                   
                }
                // tampilkan gambar hasil threshold pada picture box
                pictureBox2.Image = bitmap;
            }
        }
    }
}
