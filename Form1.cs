﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Operations;
using Colors;
using System.Linq;

namespace photoshopCsharp
{
    public partial class Form1 : Form
    {
        Control[] Controllist { get; }
        Thread thread;
        int listMapCount;
        int mx, my;
        List<int> MxList { get; }
        List<int> MyList { get; }
        Bitmap j;
        List<Bitmap> Jp { get; }

        string vybir;
        float vid;

        public Form1()
        {
            InitializeComponent();
            Controllist = new System.Windows.Forms.Control[this.Controls.Count + 1];
            Controllist[0] = textBox3;
            Controllist[1] = textBox3;
            Controllist[2] = label5;
            Controllist[3] = comboBox1;
            Controllist[4] = textBox2;
            Controllist[5] = label4;
            Controllist[6] = label3;
            Controllist[7] = label2;
            Controllist[8] = textBox5;
            Controllist[9] = textBox4;
            Controllist[10] = textBox1;
            Controllist[11] = checkBox1;
            Controllist[12] = label6;
            Controllist[13] = textBox6;
            Controllist[14] = textBox7;
            Controllist[15] = pictureBox2;

            Controllist[Controllist.Length-8] = label1;
            Controllist[Controllist.Length-7] = button4;
            Controllist[Controllist.Length-6] = button5;
            Controllist[Controllist.Length - 5] = button3;
            Controllist[Controllist.Length-4] = button2;
            Controllist[Controllist.Length-3] = button1;
            Controllist[Controllist.Length-2] = comboBox2;
            Controllist[Controllist.Length-1] = pictureBox1;
            vid = -1;
            j = new Bitmap(pictureBox1.Image);// new Bitmap(@"E:\C#\photoshopCsharp\1.jpg");
            listMapCount = 0;
            MxList = new List<int>();
            MyList = new List<int>();
            Jp = new List<Bitmap>();

            CreateMap(j);
            vybir = comboBox1.Text;
            comboBox2_SelectedIndexChanged();
        }
        void Clear()
        {
            for (int i = 0; i < Controllist.Length - 8; i++)
            {
                Controllist[i].Visible = false;
            }
            pictureBox2.BackColor = Color.FromArgb(0,0,0,0);
        }

        #region Color functions
        void Promeni(ColirRGB[] c)
        {
            int x, y;
            float s, k = Operation.mmK((float)Double.Parse(textBox1.Text.Replace('.',',')));
            float rand = Operation.mmK((float)Double.Parse(textBox2.Text.Replace('.', ',')));
            for (int i = 0; i < c.Length; i++)
            {
                x = i % mx;
                y = (int)(0.25f * my - i / mx);

                ColirHSV h = new ColirHSV(c[i]);
                s = 100 - h.s;
                h.h = Operation.mmK((int)(Math.Atan2(y, x) * 720 / Math.PI), max: 360);
                h.v = 100;
                h.s = Operation.mm((int)(100 * Math.Sqrt(x * x + y * y) / mx), max: 100);
                c[i] = ColirOperation.Mix(new ColirRGB(h), c[i], Math.Abs(RandomMy.random()) % 1000 <= (int)(1000f * rand) ? h.s * k * s * 0.0001f : 0);
            }
        }
        void Mezhi(ColirRGBA[] c)
        {
            int count = int.Parse(textBox1.Text) > 0 ? int.Parse(textBox1.Text) : 1;
            uint[] r = new uint[mx * my];
            float ch = Operation.mmK((float)Double.Parse(textBox4.Text.Replace('.', ',')));

            if (vybir == "за виглядом")
                for (int i = 0; i < c.Length; i++)
                    r[i] = (ColirOperation.d(Ceredniy(c, i % mx, i / mx), c[i]) >= ch * 255) ? 1u : 0;
            else
                for (int i = 0; i < c.Length; i++)
                    r[i] = (Operation.riznK(((ColirHSV)Ceredniy(c, i % mx, i / mx)).h, ((ColirHSV)c[i]).h, diapazon: 360) >= ch * 120) ? 1u : 0;

            for (int e = 0; e + 1 < count; e++)
                for (int i = 0; i < c.Length; i++)
                    r[i] = Mezha(r, i % mx, i / mx, (uint)(e + 1));

            for (int i = 0; i < c.Length; i++)
                if (r[i] > 0) c[i] = new ColirRGBA();

            uint Mezha(uint[] k, int x, int y, uint t)
            {
                int n = x + y * mx;
                if (k[n] > 0)
                    return k[n];
                if (n >= mx)
                {
                    if (x > 0 && k[n - 1 - mx] == t) return t + 1;
                    if (x + 1 < mx && k[n + 1 - mx] == t) return t + 1;
                    if (k[n - mx] == t) return t + 1;
                }
                if (n < mx * (my - 1))
                {
                    if (x > 0 && k[n - 1 + mx] == t) return t + 1;
                    if (x + 1 < mx && k[n + 1 + mx] == t) return t + 1;
                    if (k[n + mx] == t) return t + 1;
                }
                if (x > 0 && k[n - 1] == t) return t + 1;
                if (x + 1 < mx && k[n + 1] == t) return t + 1;
                return 0;
            }

        }
        void Art1(ColirRGB[] c)
        {
            double k = Operation.mmK((float)Double.Parse(textBox2.Text.Replace('.', ',')));
            float rand = Operation.mmK((float)Double.Parse(textBox2.Text.Replace('.', ',')));
            vid = 300;
            for (int i = 0; i < c.Length; i++)
            {
                if (Math.Abs(RandomMy.random()) % 1000 <= (int)(1000f * rand))
                {
                    c[i].R = (c[i].R / (int)(255 * k + 1) * (int)(k * 255 + 1));
                    c[i].G = (c[i].G / (int)(255 * k + 1) * (int)(k * 255 + 1));
                    c[i].B = (c[i].B / (int)(255 * k + 1) * (int)(k * 255 + 1));
                }
                vid = 300 + (int)(9600f * i / c.Length);
            }
        }
        void Art2(ColirRGB[] c)
        {
            ColirHSV[] h = new ColirHSV[c.Length];
            for (int i = 0; i < c.Length; i++)
                h[i] = new ColirHSV(c[i]);
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirRGB(h[i]);
        }
        void Ucerednenia(ColirRGBA[] c)
        {
            ColirRGBA[] cNew = new ColirRGBA[c.Length];
            for (int i = 0; i < c.Length; i++)
                cNew[i] = Ceredniy(c, i % mx, i / mx);
            for (int i = 0; i < c.Length; i++)
                c[i] = cNew[i];
        }
        void Kolir(ColirRGBA[] c)
        {
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirRGBA(((ColirHSV)(c[i])).h, 100, 100);
        }
        void Mix(ColirRGB[] c)
        {
            if (listMapCount > 1)
            {
                float kut = 1 / (float)Math.Tan((double)(Operation.diap((float)Double.Parse(textBox4.Text.Replace('.', ',')), max: 135f, min: 45f) / 57.17f));
                int x1 = (int)(mx * Operation.diap((float)Double.Parse(textBox1.Text.Replace('.', ','))));
                int w = int.Parse(textBox6.Text) < 0 ? 0 : int.Parse(textBox6.Text);

                float ky = (float)MyList.ElementAt(listMapCount - 2) / my;
                int x2 = (int)(MxList.ElementAt(listMapCount - 2) * Operation.diap((float)Double.Parse(textBox2.Text.Replace('.', ','))) / ky);
                int rx2 = (int)(MxList.ElementAt(listMapCount - 2) / ky);

                ColirRGB[] c2 = new ColirRGB[(int)(MxList.ElementAt(listMapCount - 2) / ky) * my];
                for (int i = 0; i < c2.Length; i++)
                    c2[i] = new ColirRGB(Jp.ElementAt(listMapCount - 2).GetPixel((int)(i % rx2 * ky), (int)(i / rx2 * ky)));

                int d = x1 - x2;
                int rx = (int)(rx2 + d);
                ColirRGB[] map = new ColirRGB[my * rx];

                int x, y, xk;
                float k;
                for (int i = 0; i < map.Length; i++)
                {
                    x = i % rx;
                    y = i / rx;
                    xk = x1 - (int)(y * kut);

                    if (x + w <= xk)
                        map[i] = c[y * mx + x];
                    else
                    if (x >= xk + w)
                        map[i] = c2[rx2 * y + x - d];
                    else
                    {
                        k = 0.5f + (xk - x) * 0.5f / w; //1 05 0
                        map[i] = ColirOperation.Mix((x >= mx) ? c[y * mx + mx - 1] : c[y * mx + x], (x < d) ? c2[rx2 * y] : c2[rx2 * y + x - d], k);
                    }
                }
                mx = rx;
                c = map;
            }
        }
        void osv(ColirRGBA[] c)
        {
            ColirHSV s;
            for (int i = 0; i < c.Length; i++)
            {
                s = c[i];
                c[i] = new ColirRGBA(new ColirHSV(s.h, (int)(Math.Sqrt(s.s) * 10), s.v));
            }
        }
        void PutSpace(ColirRGBA[] c)
        {
            bool all = true;
            Color space = pictureBox2.BackColor;
            int lenth = c.Where(e => space == e).Count();
            List<int> exceptions = new List<int>();

            while (true)
            {
                ColirRGBA[] newc = new ColirRGBA[c.Length];
                c.CopyTo(newc, 0);

                for (int i = 0; i < c.Length; i++)
                    if(IsSpace(i,c))
                {
                        int x = i % mx;
                        int y = i / mx;
                        int r=0,g=0,b=0;
                        int count = 0;

                        for(int yy=-2;yy<3;yy++)
                        for(int xx = -2; xx < 3; xx++)
                            {
                                if (x+xx >= 0 && x+xx < mx && y + yy >= 0 && y + yy < my && !IsSpace(i + xx + yy * mx, c))
                                {
                                    r += c[i + xx + yy*mx].R;
                                    g += c[i + xx + yy * mx].G;
                                    b += c[i + xx + yy * mx].B;
                                    count++;
                                }
                            }

                        if (count > 0)
                        {
                            newc[i] = new ColirRGBA(r/count, g/count, b/count, c[i].a);
                                vid += 10000f / lenth;
                            if(IsSpace(i,newc))
                                exceptions.Add(i);
                        }

                        all = false;
                    }
                newc.CopyTo(c, 0);
                if (all)
                    return;
                all = true;
            }

            bool IsSpace(int index, ColirRGBA[] a) {
                if (exceptions.Contains(index))
                    return false;

                int r = Math.Abs(space.R - a[index].R);
                int g = Math.Abs(space.G - a[index].G);
                int b = Math.Abs(space.B - a[index].B);

                return r + g + b < 15;
            }
        }

        public void AlphaShow(ColirRGBA[] c)
        {
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirRGBA((int)(c[i].R * c[i].a / 255f), (int)(c[i].G * c[i].a / 255f), (int)(c[i].B * c[i].a / 255f));
        }
        public void AlphaToColor(ColirRGBA[] c)
        {
            int k = int.Parse(textBox1.Text).mm(1, 50);
            int variant = int.Parse(textBox2.Text).mm(1, 6);
            int variant2 = int.Parse(textBox4.Text).mm(1, 3);
            int k1 = int.Parse(textBox7.Text).mm(1, 100);
            float rozmir = (float)(int.Parse(textBox6.Text).mm(1, 10000)) * 0.01f;
            for (int y = 0; y < my; y++)
                for (int x = 0; x < mx; x++)
                {
                    float rx = (float)x * rozmir, ry = (float)y * rozmir;
                    double nx = 2f * x * k / mx * Math.PI, ny = 2f * y * k / my * Math.PI;
                    int i = x + y * mx;
                    float ri = rx + ry * mx;
                    float cx = rx / mx, cy = ry / my;
                    //float t = 5000f * Math.Pow(Math.E, Math.Sin(i)) + x + y;
                    //int h = (7200 + (int)(10f * ind) - (int)(cx * 10f) * 360 - (int)(cy * 10f) * 360) % 360;
                    int h = 0;
                    switch (variant)
                    {
                        case 1:
                            h = (int)((ri * (ri % 10))) % 360;
                            break;
                        case 2:
                            h = (int)(5000f * Math.Pow(Math.E, Math.Sin(ri)) + rx + ry) % 360;
                            break;
                        case 3:
                            h = (7200 + (int)(10f * i) - (int)(cx * 10f) * 360 - (int)(cy * 10f) * 360) % 360;
                            break;
                        case 4:
                            h = (int)(rx * rx * rx + ry * Math.Pow(i, 2 + Math.Sin(rx * ry))) % 360;
                            break;
                        case 5:
                            h = (int)(ri * (ri % k)) % 360;
                            break;
                        case 6:
                            float kx = rx % k1 -0.5f * k1, ky = ry % k1 - 0.5f * k1;
                            float n1 = Math.Abs(kx * kx + ky * ky + k * k * 0.25f);
                            double n2 = 1 + Math.Sin(x) * Math.Cos(y);
                            h = (int)(n1 * n2) % 360;
                            break;
                    }

                    var colir = new ColirRGBA(new ColirHSV(h, 100, 100), c[i].a);

                    int a = 255;
                    switch (variant2)
                    {
                        case 1:
                            a = (int)(c[i].a * c[i].a / 255f + (int)((Math.Sin(nx + Math.PI * 0.5f) + 1f) * 0.5f * (255 - c[i].a)));
                            break;
                        case 2:
                            a = (int)(c[i].a * c[i].a / 255f + (int)((Math.Sin(ny + Math.PI * 0.5f) + 1f) * 0.5f * (255 - c[i].a)));
                            break;
                        case 3:
                            a = 255;
                            break;
                    }

                    c[i] = new ColirRGBA((int)(c[i].R * c[i].a / 255f + colir.R * (255 - c[i].a) / 255f),
                        (int)(c[i].G * c[i].a / 255f + colir.G * (255 - c[i].a) / 255f),
                        (int)(c[i].B * c[i].a / 255f + colir.B * (255 - c[i].a) / 255f),
                        a);
                    vid = 200f + 10000f * i / my / mx;
                }
            vid = 10200f;
        }
        public void ChangeSize(ref ColirRGBA[] c)
        {
            int sx = int.Parse(textBox1.Text).mm(1, 10000), sy = int.Parse(textBox2.Text).mm(1, 10000);
            ColirRGBA[] newC = new ColirRGBA[sx * sy];
            for (int y = 0; y < sy; y++)
                for (int x = 0; x < sx; x++)
                    newC[x + y * sx] = c[(int)((float)x * mx / sx) + (int)((float)y * my / sy) * mx];
            c = newC;
            mx = sx;
            my = sy;
        }

        //extra methods
        ColirRGBA Ceredniy(ColirRGBA[] c, int x, int y)
        {
            ColirRGBA cer = new ColirRGBA();
            int k = 0, n = x + y * mx;
            if (n >= mx)
            {
                if (x > 0)
                {
                    k++;
                    cer += c[n - mx - 1];
                }
                if (x + 1 < mx)
                {
                    k++;
                    cer += c[n - mx + 1];
                }
                k++;
                cer += c[n - mx];
            }
            if (n < mx * (my - 1))
            {
                if (x > 0)
                {
                    k++;
                    cer += c[n + mx - 1];
                }
                if (x + 1 < mx)
                {
                    k++;
                    cer += c[n + mx + 1];
                }
                k++;
                cer += c[n + mx];
            }
            if (x > 0)
            {
                k++;
                cer += c[n - 1];
            }
            if (x + 1 < mx)
            {
                k++;
                cer += c[n + 1];
            }
            cer = new ColirRGBA((int)((float)cer.R / k), (int)((float)cer.G / k), (int)((float)cer.B / k));
            return cer;
        }
        #endregion

        public void Do(object s)
        {
            Bitmap map = new Bitmap(j);
            ColirRGBA[] c = new ColirRGBA[mx * my];
            vid = 50f;
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirRGBA(map.GetPixel(i % mx, i / mx));

            vid = 100f;

            foreach (var element in typeof(Form1).GetMethods())
                if ((string)s == element.Name && element.GetParameters().Length == 1 && element.GetParameters()[0].ParameterType == typeof(ColirRGBA[]))
                    element.Invoke(this, new object[] { c });

            switch (s)
            {
                case "ChangeSize":
                    ChangeSize(ref c);
                    break;
                case "округлення":
                    Art1(c);
                    break;
                case "art2":
                    Art2(c);
                    break;
                case "promeni":
                    Promeni(c);
                    break;
                case "межі":
                    Mezhi(c);
                    break;
                case "усереднення":
                    Ucerednenia(c);
                    break;
                case "чисто кольоровий":
                    Kolir(c);
                    break;
                case "поєднання":
                    Mix(c);
                    break;
                case "освітлення":
                    osv(c);
                    break;
                case "заповнити пустоту":
                    PutSpace(c);
                    break;
            }
            vid += 100f;
            map = new Bitmap(map, mx, my);
            for (int i = 0; i < c.Length; i++)
                map.SetPixel(i % mx, i / mx, c[i]);
            j = new Bitmap(map);
            CreateMap(map);
            vid += 100f;
        }

        #region events
        private void button1_Click(object sender, EventArgs e)//запуск
        {
            label5.Visible = true;
            thread = new Thread(Do);
            thread.Start(comboBox2.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender = null, EventArgs e = null)
        {
            Clear();
            switch (comboBox2.Text)
            {
                case "groza":
                    comboBox1.Location = new Point(1028, 48);
                    checkBox1.Location = new Point(1028, 75);
                    label1.Text = "";

                    label2.Location = new Point(1025, 100);
                    label3.Location = new Point(1025, 125);
                    label4.Location = new Point(1025, 150);
                    label5.Location = new Point(4000, 175);

                    label2.Text = "якість";
                    label3.Text = "величина";
                    label4.Text = "кількість";

                    textBox1.Location = new Point(1098, 97);
                    textBox2.Location = new Point(1098, 122);
                    textBox3.Location = new Point(4000, 172);
                    textBox4.Location = new Point(1098, 147);
                    textBox5.Location = new Point(4000, 75);
                    break;
                case "trava":
                case "asfalt":
                    comboBox1.Location = new Point(4000, 48);
                    checkBox1.Location = new Point(4000, 86);
                    label1.Text = "";

                    label2.Location = new Point(1028, 48);
                    label3.Location = new Point(1028, 75);
                    label4.Location = new Point(1028, 102);
                    label5.Location = new Point(1028, 129);

                    label2.Text = "розмір";
                    label3.Text = "мінімум і максимум";
                    label4.Text = "значення рандома";

                    textBox1.Location = new Point(1150, 48);
                    textBox2.Location = new Point(1150, 102);
                    textBox3.Location = new Point(1150, 129);
                    textBox4.Location = new Point(1150, 75);
                    textBox5.Location = new Point(1250, 75);
                    break;
                case "r":
                    decimal r;
                    r = 10m / 3;
                    label2.Text = r.ToString();
                    break;
                case "округлення":
                case "art2":
                    label1.Visible =
                        textBox1.Visible =
                        label3.Visible =
                        textBox2.Visible =
                        true;
                    label1.Text = "коефіцієнт";
                    label3.Text = "частота";
                    textBox1.Text = "1";
                    textBox2.Text = "0";
                    label1.Enabled = true;
                    break;
                case "promeni":
                    textBox1.Visible =
                        label3.Visible =
                        textBox2.Visible =
                        true;
                    label1.Text = "коефіцієнт";
                    label3.Text = "частота";
                    textBox1.Text = "1";
                    textBox2.Text = "0";
                    break;
                case "межі":
                    textBox1.Visible =
                        label3.Visible =
                        label4.Visible =
                        textBox2.Visible =
                        textBox4.Visible =
                        comboBox1.Visible =
                        true;
                    label1.Text = "розмір меж";
                    label3.Text = "частота";
                    label4.Text = "чутливість";
                    textBox1.Text = "1";
                    textBox2.Text = "0";
                    textBox4.Text = "1";
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("за виглядом");
                    comboBox1.Items.Add("за кольором");
                    break;
                case "поєднання":
                    textBox1.Visible =
                        label3.Visible =
                        label4.Visible =
                        textBox2.Visible =
                        textBox4.Visible =
                        label6.Visible =
                        textBox6.Visible =
                        true;
                    label1.Text = "відсоток х1";
                    label3.Text = "відсоток х2";
                    label4.Text = "кут";
                    textBox1.Text = "0.5";
                    textBox2.Text = "0.5";
                    textBox4.Text = "90";
                    break;
                case "AlphaToColor":
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    textBox4.Visible = true;
                    textBox6.Visible = true;
                    textBox7.Visible = true;
                    textBox1.Text = "10";
                    textBox2.Text = "6";
                    textBox4.Text = "3";
                    textBox6.Text = "100";
                    textBox7.Text = "10";
                    break;
                case "ChangeSize":
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    textBox1.Text = "1000";
                    textBox2.Text = "1000";
                    break;
                case "заповнити пустоту":
                    comboBox1.Visible = true;
                    pictureBox2.Visible = true;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("вручну");
                    comboBox1.Items.Add("за фото");
                    comboBox1.Text = "за фото";
                    comboBox1_SelectedIndexChanged(null, null);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float nvid = vid / 1.035f;
            label5.Text = nvid == -1 ? "" : ((int)nvid / 100).ToString() + "." + ((int)nvid % 100).ToString() + "%";
            if (thread != null && thread.ThreadState != ThreadState.Running)
            {
                label5.Visible = false;
                thread.Abort();
                vid = -1;
                pictureBox1.Image = j;
            }
        }

        private void button4_Click(object sender, EventArgs e)//назад
        {
            if (listMapCount > 1)
            {
                Jp.RemoveAt(listMapCount - 1);
                MxList.RemoveAt(listMapCount - 1);
                MyList.RemoveAt(listMapCount - 1);
                listMapCount--;
                j = new Bitmap(Jp.ElementAt(listMapCount - 1));
                mx = j.Width;
                my = j.Height;
            }
            pictureBox1.Image = j;
        }

        private void button2_Click(object sender, EventArgs e)  //зберегти
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Зберегти";
            fd.OverwritePrompt = fd.ShowHelp = true;
            fd.Filter = "Image Files(*JPEG)|*.JPEG|Image Files(*PNG)|*.PNG|All Files(*.*)|*.*";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    j.Save(fd.FileName, format: System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show("невдалося зберегти");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            vybir = comboBox1.Text;
                if (comboBox2.Text == "заповнити пустоту" && vybir == "вручну")
                {
                    colorDialog1.ShowDialog();
                    pictureBox2.BackColor = colorDialog1.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)//відкрити
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Відкрити";
            fd.CheckFileExists = fd.ShowHelp = true;
            fd.Filter = "All Files(*.*)|*.*|Image Files(*JPEG)|*.JPEG|Image Files(*PNG)|*.PNG";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    j = new Bitmap(fd.FileName);
                    CreateMap(j);
                }
                catch
                {
                    MessageBox.Show("невдалося відкрити");
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(comboBox2.Text == "заповнити пустоту" && vybir == "за фото")
            {
                Bitmap picture = pictureBox1.Image as Bitmap;
                pictureBox2.BackColor = picture.GetPixel(e.X*picture.Width/pictureBox1.Width,e.Y * picture.Height / pictureBox1.Height);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            thread.Abort();
        }
        #endregion

        void CreateMap(Bitmap x1)
        {
            Bitmap x = new Bitmap(x1);
            pictureBox1.Image = x;
            mx = x.Width;
            my = x.Height;
            MxList.Add(mx);
            MyList.Add(my);
            Jp.Add(x);
            listMapCount++;
        }
    }

}
