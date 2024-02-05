using FIT.Data;
using FIT.Infrastructure;
using FIT.WinForms.Helpers;
using FIT.WinForms.Izvjestaji;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIT.WinForms.frmIB210128
{
    public partial class frmDrzaveIB210128 : Form
    {
        DLWMSDbContext db = new DLWMSDbContext();
        List<DrzavaIB210128> drzave = new List<DrzavaIB210128>();
        int rowIndex = 0;
        public frmDrzaveIB210128()
        {
            InitializeComponent();
        }

        private void frmDrzaveIB210128_Load(object sender, EventArgs e)
        {
            UcitajSve();
        }

        private void UcitajSve()
        {
            drzave = db.DrzaveIB210128.ToList();

            var dsDrzave = new DataTable();
            dsDrzave.Columns.Add("Zastava", typeof(Image));
            dsDrzave.Columns.Add("Naziv");
            dsDrzave.Columns.Add("BrojGradova");
            dsDrzave.Columns.Add("Aktivna");

            for (int i = 0; i < drzave.Count(); i++)
            {
                var noviRed = dsDrzave.NewRow();
                noviRed["Zastava"] = Ekstenzije.ToImage(drzave[i].Zastava);
                noviRed["Naziv"] = drzave[i].Naziv.ToString();
                noviRed["BrojGradova"] = db.GradoviIB210128.Include("Drzava").Where(x => x.DrzavaId == drzave[i].Id).Count();
                noviRed["Aktivna"] = drzave[i].Status == 1 ? true : false;

                dsDrzave.Rows.Add(noviRed);
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dsDrzave;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += UpdateTime;
            timer.Start();
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void UpdateTime(object? sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmNovaDrzavaIB210128 novaDrzava = new frmNovaDrzavaIB210128();
            novaDrzava.ShowDialog();
            UcitajSve();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;
            if (e.ColumnIndex == 4)
            {
                frmGradoviIB210128 frmgradovi = new frmGradoviIB210128(drzave[e.RowIndex]);
                frmgradovi.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmIzvjestaji izvjestaj = new frmIzvjestaji(drzave[rowIndex]);
            izvjestaj.ShowDialog();
        }
    }
}
