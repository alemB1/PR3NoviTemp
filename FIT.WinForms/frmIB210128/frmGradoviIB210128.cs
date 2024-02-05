using FIT.Data;
using FIT.Infrastructure;
using FIT.WinForms.Helpers;
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
    public partial class frmGradoviIB210128 : Form
    {
        DLWMSDbContext db = new DLWMSDbContext();
        private DrzavaIB210128 drzavaIB210128;

        public frmGradoviIB210128()
        {
        }

        public frmGradoviIB210128(DrzavaIB210128 drzavaIB210128)
        {
            InitializeComponent();
            this.drzavaIB210128 = drzavaIB210128;
        }

        private void frmGradoviIB210128_Load(object sender, EventArgs e)
        {
            UcitajPodatke();
        }

        private void UcitajPodatke()
        {
            var drzavaGradovi = db.GradoviIB210128.Where(x => x.DrzavaId == drzavaIB210128.Id).ToList();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;

            var dsGradovi = new DataTable();
            dsGradovi.Columns.Add("Naziv");
            dsGradovi.Columns.Add("Aktivan");

            for (int i = 0; i < drzavaGradovi.Count(); i++)
            {
                var noviRed = dsGradovi.NewRow();
                noviRed["Naziv"] = drzavaGradovi[i].Naziv.ToString();
                noviRed["Aktivan"] = drzavaGradovi[i].Status == 1 ? true:false;

                dsGradovi.Rows.Add(noviRed);
            }

            dataGridView1.DataSource = dsGradovi;
            pictureBox1.Image = Ekstenzije.ToImage(drzavaIB210128.Zastava);
            label1.Text = drzavaIB210128.Naziv.ToString();  

        }
    }
}
