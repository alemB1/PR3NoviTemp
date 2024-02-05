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
                noviRed["Aktivan"] = drzavaGradovi[i].Status == 1 ? true : false;

                dsGradovi.Rows.Add(noviRed);
            }

            dataGridView1.DataSource = dsGradovi;
            pictureBox1.Image = Ekstenzije.ToImage(drzavaIB210128.Zastava);
            label1.Text = drzavaIB210128.Naziv.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var imeGrada = textBox1.Text;
            if (ValidirajGrad())
            {
                var noviGrad = new GradIB210128
                {
                    Naziv = imeGrada,
                    DrzavaId = drzavaIB210128.Id,
                    Status = 1
                };
                db.GradoviIB210128.Add(noviGrad);
                db.SaveChanges();
                UcitajPodatke();
            }
            else
            {
                MessageBox.Show("Grad vec postoji/greska u unosu");
            }
        }

        private bool ValidirajGrad()
        {
            var gradoviCount = db.GradoviIB210128.Where(x => x.DrzavaId == drzavaIB210128.Id && x.Naziv == textBox1.Text).Count() == 0;
            return Validator.ProvjeriUnos(textBox1, err, Kljucevi.Warning) && gradoviCount;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ValidirajThread())
            {
                Thread gradoviThread = new Thread(() => GenerisiGradove());
                gradoviThread.Start();
            }
            else
            {
                MessageBox.Show("Obavezan unos");
            }
        }


        private bool ValidirajThread()
        {
            return Validator.ProvjeriUnos(textBox2, err, "Obavezan unos");
        }
        private void GenerisiGradove()
        {
            Thread.Sleep(300);
            var gradCount = int.Parse(textBox2.Text);
            var status = checkBox1.Checked == true ? 1 : 0;
            for (int i = 0; i < gradCount; i++)
            {
                var noviGrad = new GradIB210128
                {
                    Naziv = "grad" + db.GradoviIB210128.Count() + i,
                    Status = status,
                    DrzavaId = drzavaIB210128.Id
                };

                db.GradoviIB210128.Add(noviGrad);
                db.SaveChanges();
                textBox2.Invoke((MethodInvoker)delegate
                {
                    txtInfo.Text += $"Dodan novi grad u {DateTime.Now}{Environment.NewLine}";
                });
            }

            Action action = () =>
            {
                UcitajPodatke();
                MessageBox.Show($"Generisano {gradCount} poruka.");
            };

            BeginInvoke(action);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var gradovi = db.GradoviIB210128.Where(x => x.DrzavaId == drzavaIB210128.Id).ToList();
            if (e.ColumnIndex == 2) {
                gradovi[e.RowIndex].Status = (gradovi[e.RowIndex].Status == 1) ? 0 : 1;
                db.SaveChanges();
                UcitajPodatke();
            }
        }
    }
}
