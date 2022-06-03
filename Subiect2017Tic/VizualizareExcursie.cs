using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Subiect2017Tic
{
    public partial class VizualizareExcursie : Form
    {
        List<Planificare> planificari { get; set; } = new List<Planificare>();
        
        #region Planificari
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

        public void DisplayLocalitati()
        {
            dataGridView1.Rows.Clear();
            foreach (var e in planificari)
            {
                dataGridView1.Rows.Add(new object[] { e.localitate, e.dataStart?.ToShortDateString(), e.dataStop?.ToShortDateString(), e.frecventa, e.ziua });
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
            });
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
        #endregion

        #region Perioada de vizitare
        private void SetupGridVizitareLayout()
        {
            dataGridView2.Columns.Add("Nume", "Nume");
            dataGridView2.Columns.Add("DataStart", "DataStart");
            dataGridView2.Columns.Add("DataStop", "DataStop");
            dataGridView2.Columns.Add("Frecventa", "Frecventa");
            var datetime = DateTime.Now.Date;
            foreach (DataGridViewColumn x in dataGridView2.Columns)
            {
                x.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
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
                        dataGridView2.Rows.Add(e.localitate,
                            Common.Max((DateTime)e.dataStart, d1).ToShortDateString(),
                            Common.Min((DateTime)e.dataStop, d2).ToShortDateString(),
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
                            dataGridView2.Rows.Add(e.localitate,
                                t1.ToShortDateString(),
                                t1.ToShortDateString(),
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
                            dataGridView2.Rows.Add(e.localitate,
                                t.ToShortDateString(),
                                t.ToShortDateString(),
                                e.frecventa);
                        }
                    }
                }
            }
        }

        
        #endregion

        #region Itinerariu
        
        class ItinerariuEntry
        {
            public DateTime data { get; set; }
            public Localitate localitate { get; set; }
        }

        private void SetupItinerariu()
        {
            dataGridView3.Columns.Add("Nume", "Nume");
            dataGridView3.Columns.Add("Data", "Data");

            foreach (DataGridViewColumn x in dataGridView3.Columns)
            {
                x.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void AfiseazaItinerariu()
        {

            List<ItinerariuEntry> entries = new List<ItinerariuEntry>();
            foreach(DataGridViewRow r in dataGridView2.Rows)
            {
                if (r.Cells[1].Value == null || r.Cells[2].Value == null)
                    continue;

                DateTime d1 = DateTime.Parse((string)r.Cells[1].Value), d2 = DateTime.Parse((string)r.Cells[2].Value);
                for (var t = d1; t <= d2;t= t.AddDays(1))
                    entries.Add(new ItinerariuEntry() { localitate = (Localitate)r.Cells[0].Value, data = t });
            }
            entries.Sort((a, b) => a.data < b.data ? -1 : 1);

            foreach(var e in entries)
            {
                dataGridView3.Rows.Add(e.localitate, e.data.ToShortDateString());
            }
        }

        #endregion

        List<ShowImageDetails> imagesToBeShown { get; set; } = new List<ShowImageDetails>();

        #region Vizualizare Imagini

        class ShowImageDetails
        {
            public string localitate { get; set; }
            public Imagine img { get; set; }
            public DateTime data { get; set; }
        }

        private async void ShowImaginiAsync(List<ShowImageDetails> imagini, CancellationToken tk)
        {
            this.Invoke(new SetStartButtonState((state) => { ButtonStart.Text = state; }), "Stop");
            for(int i=0;i<imagini.Count;i++)
            {
                if (tk.IsCancellationRequested)
                {
                    this.Invoke(new SetStartButtonState((state) => { ButtonStart.Text = state; }), "Start");
                    return;
                }

                var img = imagini[i].img;
                img.Load();
                var progress = (int)(i / (double)(imagini.Count-1) * 100.0);
                pictureBoxItinerariu.Image = img.img;
                this.Invoke(new SetImageInfo((val,numeLocalitate,data) => { 
                    progressBar1.Value = val;
                    labelLocalitate.Text = numeLocalitate;
                    labelData.Text = data;
                }),progress,imagini[i].localitate,imagini[i].data.ToShortDateString());

                if (tk.IsCancellationRequested)
                {
                    this.Invoke(new SetStartButtonState((state) => { ButtonStart.Text = state; }), "Start");
                    return;
                }

                try
                {
                    await Task.Delay(2000,tk);

                }
                catch (TaskCanceledException ex)
                {
                    this.Invoke(new SetStartButtonState((state) => { ButtonStart.Text = state; }), "Start");
                    return;
                }
            }

            this.Invoke(new SetStartButtonState((state) => { ButtonStart.Text = state; }), "Start");
        }
        delegate void SetStartButtonState(string state);
        delegate void SetImageInfo(int val,string numeLocalitate,string data);
        class VizUnic
        {
            public int cnt { get; set; }
            public List<DateTime> date { get; set; } = new List<DateTime>();    
        }
        private void VizualizareImagini()
        {
            imagesToBeShown.Clear();

            var dicLoc = new Dictionary<Localitate, VizUnic>();

            foreach(DataGridViewRow r in dataGridView3.Rows)
            {
                var loc = (Localitate)r.Cells[0].Value;
                if (loc == null)
                    continue;
                var data = DateTime.Parse((string)r.Cells[1].Value);
                if (!dicLoc.ContainsKey(loc))
                    dicLoc.Add(loc,new VizUnic() { cnt = 0, 
                    });
                dicLoc[loc].cnt++;
                dicLoc[loc].date.Add(data);
            }
            foreach(var kv in dicLoc)
            {
                var loc = kv.Key;
                var cnt = kv.Value.cnt;
                loc.LoadImagini();
                for (int i = 0; i < cnt; i++)
                {
                    imagesToBeShown.Add(new ShowImageDetails()
                    {
                        img = loc.imagini[i % loc.imagini.Count],
                        localitate = loc.Nume,
                        data = kv.Value.date[i]
                    });
                }
            }

        }
        #endregion
        public VizualizareExcursie()
        {
            InitializeComponent();
            SetupGridLayout();
            SetupGridVizitareLayout();
            SetupItinerariu();
            LoadDataGridView();
        }

        /// Events
        private void GenerareExcursieButton_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Prima data este dupa cea de-a doua.");
                return;
            }
            tabControl1.SelectedIndex = 1;
            Filter(dateTimePicker1.Value, dateTimePicker2.Value);
            AfiseazaItinerariu();
            VizualizareImagini();
        }
        
        Task showImaginiTask;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private void ButtonStart_Click(object sender, EventArgs e)
        {

            if(showImaginiTask != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource = new CancellationTokenSource();
                showImaginiTask = null;
            }
            else
            {
                var tk = cancellationTokenSource.Token;
                showImaginiTask = Task.Run(() => ShowImaginiAsync(imagesToBeShown,tk),tk);
            }
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
