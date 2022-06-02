using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

namespace Subiect2017Tic
{
    
    public partial class Turism : Form
    {
        public static Turism Instance;
        public static string ImageFolderPath;
        public static bool DebugMode = true;
        public static CultureInfo Culture = CultureInfo.GetCultureInfo("FR-fr");
        public static SqlConnection con;
        public static SqlCommand cmd;
        public void Initializare()
        {
            // citire planificari.txt
            var text = File.ReadAllText("../../Resurse/planificari.txt");
            var randuri = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var rand in randuri)
            {
                var cuv = rand.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < cuv.Length; i++)
                    cuv[i] = cuv[i].Trim();
                var q1 = new SqlCommand("select * from Localitati where Nume=@p1", con);
                q1.Parameters.AddWithValue("p1", cuv[0]);

                var res = q1.ExecuteReader();
                string nameLoc = null;
                int idLoc = 0;
                while (res.Read())
                {
                    idLoc = (int)res[0];
                    nameLoc = (string)res[1];
                }
                res.Close();
                if (nameLoc == null)
                {
                    cmd = new SqlCommand("insert into Localitati (Nume) values (@p1)", con);
                    cmd.Parameters.AddWithValue("p1", cuv[0]);
                    cmd.ExecuteNonQuery();
                    var res2 = q1.ExecuteReader();
                    while (res2.Read())
                    {
                        idLoc = (int)res2[0];
                    }
                    res2.Close();
                }



                if (cuv[1] == Frecventa.ocazional)
                {
                    var data1 = DateTime.Parse(cuv[2], Culture);
                    var data2 = DateTime.Parse(cuv[3], Culture);

                    for (int i = 4; i < cuv.Length; i++)
                    {
                        cmd = new SqlCommand("insert into Imagini (IDLocalitate,CaleFisier) values (@p1,@p2)", con);
                        cmd.Parameters.AddWithValue("p1", idLoc);
                        cmd.Parameters.AddWithValue("p2", cuv[i]);
                        cmd.ExecuteNonQuery();
                    }

                    cmd = new SqlCommand("insert into Planificari (IDLocalitate,Frecventa,DataStart,DataStop) values (@p1,@p2,@p3,@p4)", con);
                    cmd.Parameters.AddWithValue("p1", idLoc);
                    cmd.Parameters.AddWithValue("p2", cuv[1]);
                    cmd.Parameters.AddWithValue("p3", data1);
                    cmd.Parameters.AddWithValue("p4", data2);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var ziua = int.Parse(cuv[2]);
                    for (int i = 3; i < cuv.Length; i++)
                    {
                        cmd = new SqlCommand("insert into Imagini (IDLocalitate,CaleFisier) values (@p1,@p2)", con);
                        cmd.Parameters.AddWithValue("p1", idLoc);
                        cmd.Parameters.AddWithValue("p2", cuv[i]);
                        cmd.ExecuteNonQuery();
                    }

                    cmd = new SqlCommand("insert into Planificari (IDLocalitate,Frecventa,Ziua) values (@p1,@p2,@p3)", con);
                    cmd.Parameters.AddWithValue("p1", idLoc);
                    cmd.Parameters.AddWithValue("p2", cuv[1]);
                    cmd.Parameters.AddWithValue("p3", cuv[2]);

                    cmd.ExecuteNonQuery();
                }
            }
            // selectare folder imagini

            if (DebugMode)
            {
                ImageFolderPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, "Resurse", "Imagini");
            }
            else
            {
                var fbd = new FolderBrowserDialog();
                fbd.SelectedPath = Application.StartupPath;
                fbd.ShowDialog();
                ImageFolderPath = fbd.SelectedPath;
            }
        }
        public Turism()
        {
            Instance = this;
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Subiect2017Tic.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();

            Initializare();
            InitializeComponent();


            CultureInfo ci = new CultureInfo("RO-ro");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        private void Turism_Load(object sender, EventArgs easdsa)
        {
            Form GenereazaPosterForm;
            this.Hide();
            GenereazaPosterForm = new VizualizareExcursie();
            GenereazaPosterForm.FormClosed += (s, e) => this.Close();
            GenereazaPosterForm.ShowDialog();
        }
    }

    public static class Frecventa
    {
        public static string ocazional { get => "ocazional"; }
        public static string anual { get => "anual"; }
        public static string lunar { get => "lunar"; }
    }

}
