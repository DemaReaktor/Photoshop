using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Operations;
using Colors;
//using NUnit.Framework;
//using AutoItX3Lib;
using System.IO;
//using System.Globalization;


namespace photoshopCsharp
{

    public partial class Form1 : Form
    {
        // delegate void k(string s);
        Control[] Controllist { get; }

        Thread th;
        int listMapCount;
        int mx, my;
        List<int> MxList { get; }
        List<int> MyList{get;}
        Bitmap j;
        List<Bitmap> Jp { get; }
        // int[,] xy;
        //int roz = 300;
        //int min, max;
        //int valuerandom;
        //int kil;
        //int velych;
        //int koef;
        //int rand;
        bool vybir;
        int vid;
        string Tod(string s)
        {
            string s1 = "";
            for (int i = 0; i < s.Length; i++)
                s1 += s[i] == '.' ? ',' : s[i];
            return s1;
        }
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

            Controllist[14] = label1;
            Controllist[15] = button4;
            Controllist[16] = button3;
            Controllist[17] = button2;
            Controllist[18] = button1;
            Controllist[19] = comboBox2;
            Controllist[20] = pictureBox1;
            vid = -1;
            j = new Bitmap(pictureBox1.Image);// new Bitmap(@"E:\C#\photoshopCsharp\1.jpg");
            listMapCount = 0;
            MxList = new List<int>();
            MyList = new List<int>();
            Jp = new List<Bitmap>();

            CreateMap(j);
            vybir = false;
            // Console.WriteLine(Application.StartupPath);
            comboBox2_SelectedIndexChanged();
        }
        void Clear()
        {
            for (int i = 0; i < Controllist.Length - 7; i++)
            {
                Controllist[i].Visible = false;
            }
        }
        //double rizn(double r1, double g1, double b1, double r2, double g2, double b2)
        //{
        //    return (Math.Abs(r1 - r2) + Math.Abs(g1 - g2) + Math.Abs(b1 - b2)) / 3;
        //}
        void Promeni(ColirRGB[] c)
        {
            int x, y;
            float s, k = Operation.mmK((float)Double.Parse(Tod(textBox1.Text)));
            float rand = Operation.mmK((float)Double.Parse(Tod(textBox2.Text)));
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
                //c[i] = new ColirRGB(h);
            }
        }
         void Mezhi(ColirRGB[] c)
        {
            int k = int.Parse(textBox1.Text)>0? int.Parse(textBox1.Text):1;
            uint[] r=new uint[mx*my]; 
            //float rand = Operation.mmK((float)Double.Parse(tod(textBox2.Text)));
            float ch = Operation.mmK((float)Double.Parse(Tod(textBox4.Text)));

            if (vybir)
                for (int i = 0; i < c.Length; i++)        
                    r[i] =(ColirOperation.d(Ceredniy(c,i%mx,i/mx),c[i])>=ch*255)?1u:0;
            else
                for (int i = 0; i < c.Length; i++)
                    r[i] = (Operation.riznK(((ColirHSV)Ceredniy(c, i%mx, i/mx)).h, ((ColirHSV)c[i]).h, diapazon: 360) >= ch*120) ? 1u : 0;

            for (int e = 0; e+1 < k; e++)
                for (int i = 0; i < c.Length; i++)
                    r[i] = Mezha(r, i % mx, i / mx,(uint)(e+1));

            for (int i = 0; i < c.Length; i++)
                if (r[i]>0) c[i] = new ColirRGB();
        }
        void Art1(ColirRGB[] c)
        {
            double k = Operation.mmK((float)Double.Parse(Tod(textBox2.Text)));
            float rand = Operation.mmK((float)Double.Parse(Tod(textBox2.Text)));
            vid = 300;
            for (int i = 0; i < c.Length; i++)
            {
                if(Math.Abs(RandomMy.random()) % 1000 <= (int)(1000f * rand))
                { 
                c[i].SetR(c[i].r / (int)(255 * k + 1) * (int)(k * 255 + 1));
                c[i].SetG(c[i].g / (int)(255 * k + 1) * (int)(k * 255 + 1));
                c[i].SetB(c[i].b / (int)(255 * k + 1) * (int)(k * 255 + 1));
                }
                vid = 300 + (int)(9600f * i / c.Length);
            }
        }
        void Art2(ColirRGB[] c)
        {
            //double k = Double.Parse(Tod(textBox1.Text));
            ColirHSV[] h = new ColirHSV[c.Length];
            for (int i = 0; i < c.Length; i++)
                h[i] = new ColirHSV(c[i]);
            //vid = 300;

            //for (int i = 0; i < c.Length; i++)
            //{
            //    c[i].SetR(c[i].r / (int)(255 * k + 1) * (int)(k * 255 + 1));
            //    c[i].SetG(c[i].g / (int)(255 * k + 1) * (int)(k * 255 + 1));
            //    c[i].SetB(c[i].b / (int)(255 * k + 1) * (int)(k * 255 + 1));
            //    vid = 300 + (int)(9600f * i / c.Length);
            //}
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirRGB(h[i]);
        }
        void Ucerednenia(ColirRGB[] c)
        {
            ColirRGB[] cNew = new ColirRGB[c.Length];
            for (int i = 0; i < c.Length; i++)
                cNew[i] = Ceredniy(c, i % mx, i / mx);
            for (int i = 0; i < c.Length; i++)
                c[i] =cNew[i];
            //c=cNew;
        }
        void Kolir(ColirRGB[] c) {
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirHSV(((ColirHSV)(c[i])).h,100,100);
        }
        ColirRGB[] Mix(ColirRGB[] c) {
            if (listMapCount > 1) {
                float kut = 1/(float)Math.Tan((double)(Operation.diap((float)Double.Parse(Tod(textBox4.Text)), max: 135f, min: 45f)/57.17f));
                int x1 = (int)(mx*Operation.diap((float)Double.Parse(Tod(textBox1.Text))));
                int w = int.Parse(textBox6.Text)<0?0:int.Parse(textBox6.Text);

                float ky = (float)MyList.ElementAt(listMapCount - 2) / my;
                int x2 = (int)(MxList.ElementAt(listMapCount - 2) *Operation.diap((float)Double.Parse(Tod(textBox2.Text)))/ky);
                int rx2 = (int)(MxList.ElementAt(listMapCount - 2) / ky);

                ColirRGB[] c2 = new ColirRGB[(int)(MxList.ElementAt(listMapCount - 2)/ky)*my];
                for (int i = 0; i < c2.Length; i++)
                    c2[i] = new ColirRGB(Jp.ElementAt(listMapCount-2).GetPixel((int)(i%rx2*ky), (int)(i/rx2 * ky)));
                                   
                int d=x1-x2;
                int rx = (int)(rx2+d);
                ColirRGB[] map = new ColirRGB[my*rx];

                int x, y,xk;
                float k;
                for (int i = 0; i < map.Length; i++) {
                    x = i % rx;
                    y = i / rx;
                    xk = x1 -(int)(y*kut);

                    if (x+w  <= xk)
                        //map[i] = c[y * mx + x];
                    map[i] = /*(x+w>= mx) ? c[y * mx + mx - 1] :*/ c[y * mx + x];
                    else
                    if (x >= xk+w)
                        //map[i] = c2[rx2 * y + x - d];
                        map[i] = /*(x1 + wk < 0) ? c2[rx2 * y] : */c2[rx2 * y + x - d];
                    else {
                        //k = 1-(float)Math.Pow(((float)xk - x)*0.5f / w+0.5f, 2) * 2;   //3 or 0.33f  1 05 
                        //k=(((float)xk-x)/w+1)*0.5f;
                        k = 0.5f + (xk - x) * 0.5f / w; //1 05 0
                        //1+(xk-x-w)/2w
                        map[i] = ColirOperation.Mix((x>= mx) ? c[y * mx + mx - 1] : c[y * mx + x], (x <d) ? c2[rx2 * y] : c2[rx2 * y + x - d], k);
                    }

                    //map[i] = ColirOperation.Mix(c[y * mx + x], c2[(int)((y+1)*MxList.ElementAt(listMapCount - 2) 
                    //    / ky -(MxList.ElementAt(listMapCount - 2)/ky-x2))], k); 
                   // map[i] = new ColirRGB();// c[i];
                   // my = rx;
                }
                //mx = (int)(MxList.ElementAt(listMapCount - 2) / ky);
                //my = (int)(MyList.ElementAt(listMapCount - 2) / ky);
                mx = rx;
                //mx = rx;
                // mx = 100;
                // my = 100;
                c = map;
                //c = c2;
            }
            return c;
        }
        void osv(ColirRGB[] c) {
            ColirHSV s;
            for (int i = 0; i < c.Length; i++)
            {
                s = c[i];
                c[i] = new ColirHSV(s.h,(int)(Math.Sqrt(s.s)*10),s.v);
              //  c[i] = new ColirHSV(s.h,(int)(s.s*s.s*0.01f),s.v);
            }
        }

        ColirRGB Ceredniy(ColirRGB[] c,int x,int y) {
            ColirRGB cer = new ColirRGB();
            int k = 0,n= x + y * mx;
            if (n >= mx)
            {
                if (x > 0) {
                    k++;
                    cer += c[n - mx - 1];
                }  
                if (x+1 < mx)
                {
                    k++;
                    cer += c[n - mx + 1];
                }
                k ++;
                cer += c[n-mx];
            }
            if (n < mx*(my-1))
            {
                if (x > 0)
                {
                    k++;
                    cer += c[n + mx - 1];
                }
                if (x+1 < mx)
                {
                    k++;
                    cer += c[n + mx + 1];
                }
                k ++;
                cer += c[n + mx];
            }
            if (x >0)
            {
                k ++;
                cer += c[n- 1];
            }
            if (x+1 < mx)
            {
                k++;
                cer += c[n + 1];
            }
            cer = new ColirRGB((int)((float)cer.r/k), (int)((float)cer.g / k), (int)((float)cer.b / k));
            return cer;
        }
        uint Mezha(uint[] k,int x,int y, uint t) {
            int n = x + y * mx;
            if(k[n]>0)
            return k[n];
            if (n >= mx)
            {
                if (x > 0 && k[n - 1 - mx]==t) return t+1;
                if (x + 1 < mx && k[n + 1 - mx]==t) return t+1;
                if (k[n - mx]==t) return t+1;
            }
            if (n < mx * (my - 1))
            {
                if (x > 0 && k[n - 1 + mx] == t) return t + 1;
                if (x + 1 < mx && k[n + 1 + mx] == t) return t + 1;
                if( k[n + mx] == t) return t + 1;
            }
            if (x > 0 && k[n - 1] == t) return t + 1;
            if (x + 1 < mx && k[n +1] == t) return t + 1;
            return 0;
        }

        //void zo(ref int[,] u,int x,int y,int zn)
        //{
        //    if (zn >= 100)
        //    {
        //        zo(ref u, (x + 1).diapazon(max:249), y, (int)(zn / 10f));
        //        zo(ref u, (x - 1).diapazon(max: 249), y, (int)(zn / 10f));
        //        zo(ref u, x,(y + 1).diapazon(max: 249), (int)(zn / 10f));
        //        zo(ref u, x, (y - 1).diapazon(max: 249), (int)(zn / 10f));
        //    }
        //    u[x, y] += zn;
        //}
        //Color F(Color q,Color q2,int alpha)
        //{
        //    alpha = alpha > 1000 ? 1000 : alpha < 0 ? 0 : alpha;
        //   return  Color.FromArgb(255,(int)(0.001f*(1000-alpha)*q.R+ 0.001f*q2.R*alpha), (int)(0.001f * (1000 - alpha) * q.G + 0.001f * q2.G * alpha), (int)(0.001f * (1000 - alpha) * q.B + 0.001f * q2.B * alpha));
        //}
        // void I(ref int[,] o, ref int[,] u,int pid, int x=125,int y=125) {
        //    o[x, y] = 204482646;
        //    u[x, y] = (int)(25f * pid / 6);
        //    if (pid != 1)
        //    {
        //        int min, verx, vnyz, vpravo;
        //        min = o[x - 2, y];
        //        verx = o[x, y + 2];
        //        vnyz = o[x, y - 2];
        //        vpravo = o[x + 2, y];
        //        if (min > verx)
        //            min = verx;
        //        if (min > vnyz)
        //            min = vnyz;
        //        if (min > vpravo)
        //            min = vpravo;
        //        if (min == verx)
        //            I(ref o, ref u, pid - 1, x, y + 2); else
        //             if (min == vnyz)
        //            I(ref o, ref u, pid - 1, x, y - 2);
        //        else
        //             if (min == vpravo)
        //            I(ref o, ref u, pid - 1, x+2, y );
        //        else
        //            I(ref o, ref u, pid - 1, x-2, y );

        //    }
        //}
        //void I1(ref int[,] o, ref int[,] u, int pid, int x = 125, int y = 125)
        //{
        //    o[x, y] = 204482646;
        //    u[x, y] = (int)(25f * pid / 6) + u[x, y] > 1000 ? (int)(2.5f * pid) - (int)(u[x, y] * 0.4f) : (int)(25f * pid / 6) + u[x, y];
        //    if (pid != 1)
        //    {
        //        int min, verx, vnyz, vpravo;
        //        min = o[x - 1, y];
        //        verx = o[x, y + 1];
        //        vnyz = o[x, y - 1];
        //        vpravo = o[x + 1, y];
        //        if (min > verx)
        //            min = verx;
        //        if (min > vnyz)
        //            min = vnyz;
        //        if (min > vpravo)
        //            min = vpravo;
        //        if (min == vnyz)
        //            I1(ref o, ref u, pid - 1, x, y - 1);
        //        else
        //             if (min == verx)
        //            I1(ref o, ref u, pid - 1, x, y + 1);
        //        else
        //             if (min == vpravo)
        //            I1(ref o, ref u, pid - 1, x + 1, y);
        //        else
        //            I1(ref o, ref u, pid - 1, x - 1, y);

        //    }
        //}
        //void Ups(ref Bitmap h)//, Col c, int r = 800, int x = 0, int y = 0, bool pl = true)
        //{

        //    //        
        //    Random rand = new Random();
        //    int[] ra = new int[8];
        //    ra[0] = rand.Next(0, 20000);
        //    for (int i = 1; i < 8; i++)
        //    {
        //        ra[i] = ra[i - 1] * ra[i - 1] + 9 + ra[i - 1];
        //        ra[i] = (ra[i]).diapazon(max:30000);
        //    }

        //    int r, x, y;
        //    x = (ra[0]).diapazon(max:mx);//.Next(0, pictureBox1.Image.Width);
        //    y = (ra[1]).diapazon(max: my);//.Next(0, pictureBox1.Image.Height);
        //    r = (ra[2]).diapazon(max: 860,min: 50);//.Next(40, 500);
        //    bool pl = ra[3]> 1073741823 ? true:false;//.Next(0, 2) == 0 ? true : false;

        //    ColirRGB c =new ColirRGB((ra[4]).diapazon(max: 128), (ra[5]).diapazon(max: 128), (ra[6]).diapazon(max: 128));

        //    for (int yy = 0; yy < r; yy++)
        //    {
        //        float e =( (float)yy / (float)r);
        //        int ee = (int)(r * 2 * Math.Sqrt(e - e * e));

        //        for (int xx = 0; xx < ee; xx++)
        //        {
        //            float d = (yy - r/2.0f) * (yy - r/2.0f) +(xx-ee/2.0f)*(xx-ee/2.0f);
        //            int f = (xx+yy*  400 * d / r / r).diapazon(max:999);
        //            if ( f%12<=4||f%4==0) 
        //                h.SetPixel((xx + x - ee / 2).diapazon(max: mx-1),
        //                    (yy + y - r / 2).diapazon(max:my-1),
        //                    plus(h.GetPixel((xx + x - ee / 2).diapazon(max: mx-1),
        //                    (yy + y - r / 2).diapazon(max: my-1)), c, pl, 1 - 4 * d / r / r)); 
        //        }
        //    }
        //}
        //Color plus(Color a,ColirRGB b,bool pl=true,double alpha=1)
        //{
        //    //alpha = Math.Sqrt(alpha);
        //    if (pl) {
        //        return  Color.FromArgb(255, (a.R+b.r*alpha).diapazon(max:255), (a.G + b.g * alpha).diapazon(max: 255), (a.B + b.b * alpha).diapazon(max: 255));
        //    }
        //    else
        //    {
        //        return  Color.FromArgb(255,( a.R - b.r * alpha).diapazon(max: 255), (a.G - b.g * alpha).diapazon(max: 255), (a.B - b.b * alpha).diapazon(max: 255));
        //    }
        //}

        public void Do(object s)
        {
            Bitmap map = new Bitmap(j);
            ColirRGB[] c = new ColirRGB[mx * my];
            vid = 100;
            for (int i = 0; i < c.Length; i++)
                c[i] = new ColirRGB(map.GetPixel(i % mx, i / mx));
            vid = 200;
            switch (s)
            {
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
                    c=Mix(c);
                    break;
                case "освітлення":
                    osv(c);
                    break;
            }
            vid = 9900;
            map = new Bitmap(map,mx,my);
            for (int i = 0; i < c.Length; i++)
                map.SetPixel(i % mx, i / mx, c[i]);
            //pictureBox1.Image = map;
            //Jp.Add(map);  
            j = new Bitmap(map);
            //CreateMap(map);
            CreateMap(map);
            vid = 10000;
        }
        private void button1_Click(object sender, EventArgs e)//запуск
        {
            label5.Visible = true;
            th = new Thread(Do);
            th.Start(comboBox2.Text);


            //Operation.randomizationPY(out c);
            // randomizationPY();
            //  int c = 10;
            // c.randomizationPY();
            // int c = RandomMy.random();//Operation.randomizationPY();
            // button1.Text = c.ToString();
            /*
            switch (comboBox2.Text)
            {
                case "groza":
                    //не гроза
                    //xy = new int[roz, roz];
                    //for (int y = 0; y < roz; y++)
                    //    for (int x = 0; x < roz; x++)
                    //        xy[x, y] = 255;
                    koef=Int32.Parse(textBox1.Text);
                    velych=Int32.Parse(textBox2.Text);
                    kil=Int32.Parse(textBox4.Text);
                    mx = (int)(j.Width*koef*0.01f);
                    my = (int)(j.Height*koef*0.01f);
                    Bitmap h = new Bitmap(mx, my);
                        rand = new Random().Next(100, 1000);
                        int v = 1;
                        int[,] val = new int[mx, my];
                        int[,] inte = new int[mx, my];
                        int[,] alpha = new int[mx, my];
                        for (int y = 0; y < my; y++)
                            for (int x = 0; x < mx; x++)
                            {
                                val[x, y] = v;
                                v.randomization(min:1,max:1000, rand, x,y);
                                inte[x, y] = 0;
                            }
                        for (int i = kil; i >= 1; i--)//15
                            I1(ref val, ref inte, (int)(velych * koef*0.01f));

                        for (int y = 0; y < my; y++)
                            for (int x = 0; x < mx; x++)
                                if (inte[x, y] >= 100)
                                    zo(ref inte, x, y, inte[x, y]);

                        for (int i = kil; i >= 1; i--)//15
                            I(ref val, ref inte, (int)(velych * koef * 0.01f));

                        for (int y = 0; y < my; y++)
                            for (int x = 0; x < mx; x++)
                            {

                                int h1, aa;
                                switch (comboBox1.Text)
                                {
                                    case "hsv":
                                        h1 = (int)((Math.Sin(Math.PI * 2 * y / my) + (float)x / mx + 1) * 120);
                                        aa = (int)(255 * (h1 % 60) / 60);
                                        switch ((int)(h1 / 60) % 6)
                                        {
                                            case 0:
                                                h.SetPixel(x, y, Color.FromArgb(255, 255, aa, 0));
                                                break;
                                            case 1:
                                                h.SetPixel(x, y, Color.FromArgb(255, 255 - aa, 255, 0));
                                                break;
                                            case 2:
                                                h.SetPixel(x, y, Color.FromArgb(255, 0, 255, aa));
                                                break;
                                            case 3:
                                                h.SetPixel(x, y, Color.FromArgb(255, 0, 255 - aa, 255));
                                                break;
                                            case 4:
                                                h.SetPixel(x, y, Color.FromArgb(255, aa, 0, 255));
                                                break;
                                            case 5:
                                                h.SetPixel(x, y, Color.FromArgb(255, 255, 0, 255 - aa));
                                                break;
                                        }
                                        break;
                                    case "hsvRayduga":
                                        h1 = (int)((float)x / mx * y / my * 360);
                                        aa = (int)(255 * (h1 % 60) / 60);
                                        switch ((int)(h1 / 60) % 6)
                                        {
                                            case 0:
                                                h.SetPixel(x, y, Color.FromArgb(255, 255, aa, 0));
                                                break;
                                            case 1:
                                                h.SetPixel(x, y, Color.FromArgb(255, 255 - aa, 255, 0));
                                                break;
                                            case 2:
                                                h.SetPixel(x, y, Color.FromArgb(255, 0, 255, aa));
                                                break;
                                            case 3:
                                                h.SetPixel(x, y, Color.FromArgb(255, 0, 255 - aa, 255));
                                                break;
                                            case 4:
                                                h.SetPixel(x, y, Color.FromArgb(255, aa, 0, 255));
                                                break;
                                            case 5:
                                                h.SetPixel(x, y, Color.FromArgb(255, 255, 0, 255 - aa));
                                                break;
                                        }
                                        break;
                                    case "rg1":
                                        h.SetPixel(x, y, Color.FromArgb(255, (int)(255f * x / mx), (int)(255f * y / my), 255));
                                        break;
                                }
                            }

                        if (checkBox1.Checked)
                            for (int i = 0; i < 30; i++)
                                Ups(ref h);

                        for (int y = 0; y < j.Height; y++)
                            for (int x = 0; x < j.Width; x++)
                            {
                                j.SetPixel(x, y, F(j.GetPixel(x, y), h.GetPixel((int)(x / 2), (int)(y / 2)), inte[(int)((float)x / mx * 125), (int)((float)y / my * 125)]));
                            }
                    pictureBox1.Image = j;
                    break;
                case "asfalt":
                     th = new Thread(new ThreadStart(Do));
                    th.Start();
                   // label1.Text = "";
                    break;
            }
            */
            //roz = Int32.Parse(textBox1.Text);
            //min = Int32.Parse(textBox4.Text);
            //max = Int32.Parse(textBox5.Text);
            //valuerandom = Int32.Parse(textBox2.Text);
            //if (roz < 100)
            //    roz = 100;
            //if (min < 0)
            //    min = 0;
            //if (max < min)
            //    max = min;
            //if (max > 255)
            //    max = 255;

            //j = new Bitmap(roz, roz);

            //int rand = new Random().Next(min,max);

            //for (int y = 0; y < roz; y++)
            //    for (int x = 0; x < roz; x++)
            //    {
            //        j.SetPixel(x, y, Color.FromArgb(255, 0, (int)(((Math.Sin((roz*0.5f-x)*0.02f*Math.PI)*0.5f+0.5f)*(max-min) + min) * (100 - valuerandom) * 0.005f+rand* valuerandom * 0.01f), 0));
            //        rand.randomization( k1:x * y + x * 2 + 3,max:255);
            //    }   
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
                        comboBox1.Visible=
                        true;
                    label1.Text = "розмір меж";
                    label3.Text = "частота";
                    label4.Text = "чутливість";
                    textBox1.Text = "1";
                    textBox2.Text = "0";
                    textBox4.Text = "1";
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
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = vid == -1 ? "" : (vid / 100).ToString() + "." + (vid % 100).ToString() + "%";
            if (label5.Text == "100%")
            {
                label5.Visible = false;
                th.Abort();
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

        //  void Process() { }
        //void Do()
        //{
        //    vid = 0;
        //    roz = (Int32.Parse(textBox1.Text)).diapazon(max: 1000);
        //    min = (Int32.Parse(textBox4.Text)).diapazon(max: 100);
        //    max = (Int32.Parse(textBox5.Text)).diapazon(max: 100);
        //    valuerandom = (Int32.Parse(textBox1.Text)).diapazon();
        //    j = new Bitmap(roz, roz);
        //    rand = new Random().Next(min, max);
        //    int hh;
        //    hh = (Int32.Parse(textBox3.Text)).diapazon(max: 360);

        //    ColirHSV colir = new ColirHSV(hh, 0, 0);
        //    vid = 1;
        //    for (int y = 0; y < roz; y++)
        //        for (int x = 0; x < roz; x++)
        //        {
        //            //j.SetPixel(x, y, Color.FromArgb(255, 0, (int)(((Math.Sin((roz * 0.5f - x) * 0.02f * Math.PI) * 0.5f + 0.5f) * (max - min) + min) * (100 - valuerandom) * 0.005f + rand * valuerandom * 0.01f), 0));
        //            colir.v = rand;
        //            Color color = new Color();
        //            color.FromColir(colir);
        //            j.SetPixel(x, y, color);

        //            rand.randomization(delegat: delegate (ref int c, int k1, int k2, int k3) {
        //                k2.randomization(k1:9,k2:x,max:100);
        //                c =c*k1+k2+3; //c + y * x+x*9+17+y*3+c*x*2+c*y;
        //            }, k1: y, k2: roz+x, max: max, min: min,povtor:3) ;
        //           vid = (int)(1 + 99 * (x + y * roz) / roz / roz);
        //        }
        //    vid = 100;
        //}
        private void button2_Click(object sender, EventArgs e)  //зберегти
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Зберегти";
            /*fd.CheckFileExists =*/
            fd.OverwritePrompt = fd.ShowHelp = true;
            fd.Filter = "Image Files(*JPEG)|*.JPEG|Image Files(*PNG)|*.PNG|All Files(*.*)|*.*";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //if (!File.Exists(fd.FileName))
                    //    File.Create(fd.FileName,j.);
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
            vybir =(comboBox1.Text == "за виглядом");
        }

        private void button3_Click(object sender, EventArgs e)//відкрити
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Відкрити";
            fd.CheckFileExists = fd.ShowHelp = true;
            fd.Filter = "Image Files(*JPEG)|*.JPEG|Image Files(*PNG)|*.PNG|All Files(*.*)|*.*";
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
        void CreateMap(Bitmap x1) {
            Bitmap x = new Bitmap(x1);
            pictureBox1.Image =x;
            mx = x.Width;
            my = x.Height;
            MxList.Add(mx);
            MyList.Add(my);
            Jp.Add(x);
            listMapCount++;
        }
    }

}
