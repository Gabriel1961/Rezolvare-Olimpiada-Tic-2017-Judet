using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Subiect2017Tic
{
    public partial class GenereazaPoster : Form
    {
        public static GenereazaPoster Instance;
        List<Localitate> localitati = new List<Localitate>();
        SqlConnection con = Turism.con;

        void LoadLocalitati()
        {
            SqlCommand cmd = new SqlCommand("select * from Localitati",con);
            var res = cmd.ExecuteReader();
            while (res.Read())
            {
                localitati.Add(new Localitate((int)res[0],(string)res[1]));
            }
            res.Close();
            ComboLocalitati.Items.Clear();
            foreach(var loc in localitati)
            {
                loc.LoadImagini();
                ComboLocalitati.Items.Add(loc);
            }
            
            ComboLocalitati.SelectedIndex = 0;
            ComboImagine_SelectedValueChanged(ComboLocalitati,null);
            ComboImagine_SelectedIndexChanged(ComboImagine, null);
        }

        public GenereazaPoster()
        {
            Instance = this;
            InitializeComponent();
            LoadLocalitati();
            ComboLocalitati.SelectedIndexChanged += ComboImagine_SelectedValueChanged;
            ComboImagine.SelectedIndexChanged += ComboImagine_SelectedIndexChanged;
            PicBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        }

        private void ComboImagine_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = (ComboBox)sender;
            PicBoxMain.Image = 
            localitati[ComboLocalitati.SelectedIndex].imagini[obj.SelectedIndex].Load();
        }

        private void ComboImagine_SelectedValueChanged(object sender, EventArgs e)
        {
            var obj = (ComboBox)sender;
            localitati[obj.SelectedIndex].Select(ComboImagine);
        }

        private void AdaugaButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (ImageListBox.Items.Count >= 10)
                    return;
                var tba = localitati[ComboLocalitati.SelectedIndex].imagini[ComboImagine.SelectedIndex];
                if (ImageListBox.Items.IndexOf(tba) == -1)
                    ImageListBox.Items.Add(tba);
            }
            catch (Exception)
            {

            }
        }



        void CreeazaPoster(string path,string name)
        {
            const int spacing = 20;
            int maxWidth = 0, totalHeight = 0;
            foreach (var obj in ImageListBox.Items)
            {
                Imagine img = (Imagine)obj;
                maxWidth = Math.Max(maxWidth, img.img.Width);
            }
            foreach (var obj in ImageListBox.Items)
            {
                Imagine img = (Imagine)obj;
                float ratio = maxWidth / img.img.Width;
                int newHeight = (int)(img.img.Height * ratio);
                totalHeight += spacing + newHeight;
            }
            using (var bitmap = new Bitmap(maxWidth, totalHeight))
            {
                int currentHeight = 0;
                using (var canvas = Graphics.FromImage(bitmap))
                {
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    foreach (var obj in ImageListBox.Items)
                    {
                        Imagine img = (Imagine)obj;
                        float ratio = bitmap.Width/ img.img.Width;
                        int newHeight = (int)(img.img.Height * ratio);
                        canvas.DrawImage(img.img, 0, currentHeight,bitmap.Width,newHeight);
                        currentHeight += spacing + newHeight;
                    }
                }

                var dialog = new SaveFileDialog();
                dialog.FileName = name + ".png";
                dialog.InitialDirectory = path;
                dialog.Filter = "png files (*.png) | All files (*.*)";
                dialog.RestoreDirectory = true;
                Stream stream;
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    if((stream = dialog.OpenFile()) != null)
                    {
                        bitmap.Save(stream,ImageFormat.Png);

                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }

        private void GenereazaButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TitluPosterTb.Text))
            {
                MessageBox.Show("Introduceti va rog un titlu corespunzator");
                return;
            }
            else if (ImageListBox.Items.Count == 0)
            { 
                MessageBox.Show("Introduceti o imagine folosind butonul adauga");
                return;
            }
            CreeazaPoster(Turism.ImageFolderPath,TitluPosterTb.Text);
        }
    }
    public class Imagine
    {
        public Image img { get; set; }
        public int IDImagine { get; set; }
        public int IDLocalitate { get; set; }
        public string CaleFisier { get; set; }
        public Image Load()
        {
            if (img != null)
                return img;
            img = Image.FromFile(Turism.ImageFolderPath + "\\" + CaleFisier);
            return img;
        }
        public override string ToString()
        {
            
            return CaleFisier;
        }
    }

    public class Localitate
    {
        public int IDLocalitate;
        public string Nume;
        public List<Imagine> imagini = new List<Imagine>();
        public void Select(ComboBox cbx)
        {
            cbx.Items.Clear();
            foreach(Imagine i in imagini)
            {
                cbx.Items.Add(i);
            }
            if(imagini.Count > 0)
            cbx.SelectedIndex = 0;
        }
        public Localitate(int IDLocalitate, string Nume)
        {
            this.IDLocalitate = IDLocalitate;    
            this.Nume=Nume;
        }
        public void LoadImagini()
        {
            if (imagini.Count > 0)
                return;
            SqlCommand cmd = new SqlCommand("select * from Imagini where IDLocalitate=@p1",Turism.con);
            cmd.Parameters.AddWithValue("p1", IDLocalitate);
            var res = cmd.ExecuteReader();
            while (res.Read())
            {
                imagini.Add(new Imagine()
                {
                    IDImagine = (int)res[0],
                    IDLocalitate = (int)res[1],
                    CaleFisier = (string)res[2],
                });
            }
            res.Close();
        }
        public override string ToString()
        {
            return Nume;
        }
    }
}
