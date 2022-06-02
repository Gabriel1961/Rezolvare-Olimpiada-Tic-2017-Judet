using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Subiect2017Tic
{
    public partial class VizualizareExcursie : Form
    {
        List<Planificare> planificari = new List<Planificare>();
        private void SetupGridLayout()
        {
            dataGridView1.Columns.Add("Nume", "Nume");
            dataGridView1.Columns.Add("DataStart", "DataStart");
            dataGridView1.Columns.Add("DataStop", "DataStop");
            dataGridView1.Columns.Add("Frecventa", "Frecventa");
            dataGridView1.Columns.Add("Ziua", "Ziua");

            foreach(DataGridViewColumn x in dataGridView1.Columns)
            {
                x.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void SetupGridVizitareLayout()
        {
            dataGridView2.Columns.Add("Nume", "Nume");
            dataGridView2.Columns.Add("DataStart", "DataStart");
            dataGridView2.Columns.Add("DataStop", "DataStop");
            dataGridView2.Columns.Add("Frecventa", "Frecventa");

            foreach (DataGridViewColumn x in dataGridView2.Columns)
            {
                x.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        public void DisplayLocalitati()
        {
            dataGridView1.Rows.Clear();
            foreach(var e in planificari)
            {
                dataGridView1.Rows.Add(new object[] { e.localitate.Nume, e.dataStart, e.dataStop, e.frecventa, e.ziua });
            }
        }

        private void AddPlanificare(SqlDataReader red)
        {
            DateTime? date1 = null, date2 = null;
            int ziua = 0;
            if (red[1] != DBNull.Value)
                date1 = (DateTime)red[1];
            if (red[2] != DBNull.Value)
                date2 = (DateTime)red[2];
            if (red[4] != DBNull.Value)
                ziua = (int)red[4];
            planificari.Add(new Planificare()
            {
                dataStart = date1,
                dataStop = date2,
                ziua = ziua,
                frecventa = (string)red[3],
                localitate = new Localitate((int)red[5], (string)red[0])
            }); ;
        }
        public void LoadDataGridView()
        {
            var con = Turism.con;
            SqlCommand cmd = new SqlCommand("select Nume,DataStart,DataStop,Frecventa,Ziua,Localitati.IDLocalitate from Planificari inner join Localitati on Localitati.IDLocalitate=Planificari.IDLocalitate", con);
            var red = cmd.ExecuteReader();
            
            while (red.Read())
            {
                AddPlanificare(red);
            }
            red.Close();
            DisplayLocalitati();
        }

        public bool IsOverlapping(DateTime d1,DateTime d2, DateTime a, DateTime b)
        {
            return !(a > d2 || b < d1);
        }
        
        public void Filter(DateTime d1, DateTime d2)
        {
            dataGridView2.Rows.Clear();
            foreach (var e in planificari)
            {
                if(e.dataStart != null && e.dataStop != null)
                {
                    if (IsOverlapping(d1, d2, (DateTime)e.dataStart, (DateTime)e.dataStop))
                    {
                        dataGridView2.Rows.Add(e.localitate.Nume,
                            Common.Max((DateTime)e.dataStart, d1),
                            Common.Min((DateTime)e.dataStop, d2),
                            e.frecventa);

                    }
                }
                else
                {
                    if(e.frecventa == Frecventa.anual)
                    {
                        int y1 = d1.Year, y2 = d2.Year;
                        for (int i = y1; i <= y2; i++)
                        {
                            var t1 = DateTime.Parse($"1/1/{i}").AddDays(e.ziua - 1);
                            if (!(d1 <= t1 && t1 <= d2))
                                continue;
                            dataGridView2.Rows.Add(e.localitate.Nume,
                                t1,
                                t1,
                                e.frecventa);
                        }
                    }
                    else if (e.frecventa == Frecventa.lunar)
                    {
                        for(DateTime t1 = d1;t1 <= d2; t1=t1.AddMonths(1))
                        {
                            if(DateTime.DaysInMonth(t1.Year,t1.Month) < e.ziua)
                                continue;
                            var t = t1.AddDays(-t1.Day + e.ziua);
                            if (!(d1 <= t && t <= d2))
                                continue;
                            dataGridView2.Rows.Add(e.localitate.Nume,
                                t,
                                t,
                                e.frecventa);
                        }
                    }
                }
            }
        }

        public VizualizareExcursie()
        {
            InitializeComponent();
            SetupGridLayout();
            SetupGridVizitareLayout();
            LoadDataGridView();
        }

        private void GenerareExcursieButton_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Prima data este dupa cea de-a doua.");
                return;
            }
            tabControl1.SelectedIndex = 1;
            Filter(dateTimePicker1.Value, dateTimePicker2.Value);
        }
    }

    public class Planificare
    {
        public DateTime? dataStart { get; set; }
        public DateTime? dataStop { get; set; }
        public string frecventa { get; set; }
        public int ziua { get; set; }
        public Localitate localitate { get; set; }
    }


}
