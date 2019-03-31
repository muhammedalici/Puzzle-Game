using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
    
        ArrayList images = new ArrayList();
        String yol;
        int tik = 0;
        Point buton1;
        Bitmap[] parca= new Bitmap[16];
        int pixSayac=0;
        int resimSayac = 0;
        int skor = 100;
        public  string userName="asdsad";
        int[] skorlar=new int[10000];
        string dosyayolu = @"C:\\Users\\Fsociety\\source\\repos\\WindowsFormsApp1\\enyüksekskor.txt";

        public Form1()
        {
            
            InitializeComponent();
            //string yol = @"C:\\Users\\Fsociety\\Desktop\\enyüksekskor.txt";
            dosyayolu = @"C:\\Users\\Fsociety\\source\\repos\\WindowsFormsApp1\\enyüksekskor.txt";
            string[] satirlar = new string[10];
            satirlar = File.ReadAllLines(dosyayolu);
            int max = 0;
            string eniyi = "";
            for (int i = 0; i < satirlar.Length; i++)
            {
                string[] words = satirlar[i].Split('-');
                try
                {
                    skorlar[i] = Int32.Parse(words[1]);

                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{words[1]}'");
                }


                if (skorlar[i] > max)
                {
                    eniyi = satirlar[i];
                    max = skorlar[i];
                }
            }

            string ss = max.ToString();
            //label4.Text = ss;//isimsiz
            label4.Text = eniyi; //isimli

        }



        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void resimAçToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp;*.png";
                dlg.Title = "Puzzle";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box  
                    Bitmap original = new Bitmap(dlg.FileName);
                    original = new Bitmap(400, 400);
                    // image file path  
                    yol = dlg.FileName;
                   
                }
            }
        }
        
        private void cropImageTomages(Image orginal, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);

            Graphics graphic = Graphics.FromImage(bmp);

            graphic.DrawImage(orginal, 0, 0, w, h);

            graphic.Dispose();

            int movr = 0, movd = 0;

            for (int x = 0; x < 16; x++)
            {
                Bitmap piece = new Bitmap(100, 100);

                for (int i = 0; i < 100; i++)
                    for (int j = 0; j < 100; j++)
                        piece.SetPixel(i, j,
                            bmp.GetPixel(i + movr, j + movd));

                images.Add(piece);
                parca[x] = piece;
                movr += 100;

                if (movr == 400)
                {
                    movr = 0;
                    movd += 100;
                }
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") MessageBox.Show("Lüften kullanıcı adınızı giriniz");
            else
            {
                textBox1.Visible = false;
                label1.Visible = false;
                label2.Visible = true;
                label5.Visible = true;
                label5.Text = skor.ToString();

                userName = textBox1.Text;

                foreach (Button b in panel1.Controls)
                    b.Enabled = true;
                if (yol == null) MessageBox.Show("Lütfen önce resim seçiniz.");
                else
                {
                    Image orginal = Image.FromFile(yol);

                    cropImageTomages(orginal, 400, 400);

                    AddImagesToButtons(images);
                    int i = 15;

                    foreach (Button b in panel1.Controls)
                    {

                        Bitmap but = new Bitmap(b.Image);

                        for (int k = 0; k < 100; k++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (but.GetPixel(k, j) == parca[i].GetPixel(k, j)) pixSayac++;
                            }
                        }
                        if (pixSayac == 10000) resimSayac++;
                        pixSayac = 0;
                        i--;
                    }
                    //resimSayac = resimSayac / 10000;
                    if (resimSayac == 0) MessageBox.Show("Herhangi bir yer eşleştirilemedi.. \n Lütfen yeniden karıştır butonuna basınız...");
                    else
                    {
                        /* Form2 f = new Form2();
                         f.ShowDialog();
                         textBox1.Text = userName;*/
                        MessageBox.Show("oyuna başladınız..");
                        button17.Visible = false;
                    }
                    if (resimSayac == 16)
                    {
                        MessageBox.Show("tebrikler ben böyle bir bal görmedim.");
                        skor = 100;
                    }
                    resimSayac = 0;
                }
                
            }

        }
        private void AddImagesToButtons(ArrayList images)
        {
            int i = 0;
            int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            arr = suffle(arr);
            
            foreach (Button b in panel1.Controls)
            {
                if (i < arr.Length)
                {
                    
                    b.Image = (Image)images[arr[i]];
                    i++;
                }
            }
        }
        private int[] suffle(int[] arr)
        {
            Random rand = new Random();
            arr = arr.OrderBy(x => rand.Next()).ToArray();
            return arr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if(tik == 1)
            {
                tik++;
                buton1 = button1.Location;//bulunduğumuz buton
            }
            if (tik == 2)
            {
             
                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1&&btn.Location!=button1.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button1.Location.Y / 100) * 4 + button1.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button1.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button1.Location;
                        button1.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button2.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button2.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button2.Location.Y / 100) * 4 + button2.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button2.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button2.Location;
                        button2.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button3.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button3.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button3.Location.Y / 100) * 4 + button3.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button3.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button3.Location;
                        button3.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button4.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button4.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button4.Location.Y / 100) * 4 + button4.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button4.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button4.Location;
                        button4.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button5.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button5.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button5.Location.Y / 100) * 4 + button5.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button5.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button5.Location;
                        button5.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button6.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button6.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button6.Location.Y / 100) * 4 + button6.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button6.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button6.Location;
                        button6.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button7.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button7.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button7.Location.Y / 100) * 4 + button7.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button7.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button7.Location;
                        button7.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button8.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button8.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button8.Location.Y / 100) * 4 + button8.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button8.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();

                        btn.Location = button8.Location;
                        button8.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button9.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button9.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button9.Location.Y / 100) * 4 + button9.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button9.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button9.Location;
                        button9.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button10.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button10.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button10.Location.Y / 100) * 4 + button10.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button10.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button10.Location;
                        button10.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button11.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button11.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button11.Location.Y / 100) * 4 + button11.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button11.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button11.Location;
                        button11.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button12.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button12.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button12.Location.Y / 100) * 4 + button12.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button12.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button12.Location;
                        button12.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);

                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button13.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button13.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button13.Location.Y / 100) * 4 + button13.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button13.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button13.Location;
                        button13.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button14.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button14.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button14.Location.Y / 100) * 4 + button14.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button14.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button14.Location;
                        button14.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button15.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button15.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button15.Location.Y / 100) * 4 + button15.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button15.Image);
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button15.Location;
                        button15.Location = buton1;
                        tik = 1;

                    }
                }
                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;


                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i, j) == butt.GetPixel(i, j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount = 0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: " + skor);
                    File.AppendAllText(dosyayolu, userName + "-" + skor + Environment.NewLine);
                }

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            skor--;
            label5.Text = skor.ToString();
            if (tik == 0) tik++;
            if (tik == 1)
            {
                tik++;
                buton1 = button16.Location;
            }
            if (tik == 2)
            {

                foreach (Button btn in panel1.Controls)
                {
                    if (btn.Location == buton1 && btn.Location != button16.Location)
                    {
                        int sayace = 0, sayacy = 0;
                        int yere = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                        int yery = (button16.Location.Y / 100) * 4 + button16.Location.X / 100;
                        Bitmap bte = new Bitmap(btn.Image);
                        Bitmap bty = new Bitmap(button16.Image);  
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                if (parca[yere].GetPixel(i, j) == bty.GetPixel(i, j)) sayace++;
                                if (parca[yery].GetPixel(i, j) == bte.GetPixel(i, j)) sayacy++;

                            }
                        }
                        if (sayace == 10000) skor++;
                        label5.Text = skor.ToString();
                        if (sayacy == 10000) skor++;
                        label5.Text = skor.ToString();
                        btn.Location = button16.Location;
                        button16.Location = buton1;
                        tik = 1;
                        

                    }
                }

                int count = 0, index;
                int pixcaount = 0;
                foreach (Button btn in panel1.Controls)
                {
                    Bitmap butt = new Bitmap(btn.Image);
                    index = (btn.Location.Y / 100) * 4 + btn.Location.X / 100;
                    
                      
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            if (parca[index].GetPixel(i,j) == butt.GetPixel(i,j))
                            {
                                pixcaount++;
                            }
                        }
                    }
                    if (pixcaount == 10000) count++;
                    pixcaount =0;
                }
                if (count == 16)
                {
                    MessageBox.Show("Tebrikler Kazandınız... \n Skorunuz: "+ skor);
                    File.AppendAllText(dosyayolu, userName+"-"+skor + Environment.NewLine);


                }
                    


            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
