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
    public partial class frmNovaDrzavaIB210128 : Form
    {
        DLWMSDbContext db = new DLWMSDbContext();
        public frmNovaDrzavaIB210128()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validiraj())
            {
                var zastava = pictureBox1.Image;
                var naziv = textBox1.Text;
                var status = checkBox1.Checked == true ? 1 : 0;

                var novaDrzava = new DrzavaIB210128
                {
                    Naziv = naziv,
                    Zastava = Ekstenzije.ToByteArray(zastava),
                    Status = status
                };

                db.DrzaveIB210128.Add(novaDrzava);
                db.SaveChanges();
                MessageBox.Show("Uspjesno dodana nova drzava.");
            }
        }

        private bool Validiraj()
        {
            return Validator.ProvjeriUnos(pictureBox1, err, "Unesite sliku") &&
                Validator.ProvjeriUnos(textBox1, err, "Unesite ime drzave");
        }
    }
}
