﻿using FIT.Data;
using FIT.Infrastructure;
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
    public partial class frmPretragaIB210128 : Form
    {
        DLWMSDbContext db = new DLWMSDbContext();
        List<Student> studenti = new List<Student>();
        public frmPretragaIB210128()
        {
            InitializeComponent();
        }

        private void frmPretragaIB210128_Load(object sender, EventArgs e)
        {
            UcitajPodatke();
        }

        private void UcitajPodatke()
        {
            studenti = db.Studenti.ToList();
            var dsStudenti = new DataTable();
            dsStudenti.Columns.Add("Ime");
            dsStudenti.Columns.Add("Prezime");
            dsStudenti.Columns.Add("Grad");
            dsStudenti.Columns.Add("Drzava");
            dsStudenti.Columns.Add("Prosjek");

            for (int i = 0; i < studenti.Count(); i++)
            {
                var ImeGrada = db.GradoviIB210128.FirstOrDefault(x => x.Id == studenti[i].GradId).Naziv.ToString();
                var ImeDrzave = db.DrzaveIB210128.FirstOrDefault(x => x.Id == studenti[i].Grad.DrzavaId).Naziv.ToString();
                // iskreno veoma messy ali ostavim kako jest jebiga

                var prosjekCheck = db.PolozeniPredmeti.Where(x => x.StudentId == studenti[i].Id).ToList();

                    

                var noviRed = dsStudenti.NewRow();
                noviRed["Ime"] = studenti[i].Ime.ToString();
                noviRed["Prezime"] = studenti[i].Prezime.ToString();
                noviRed["Grad"] = ImeGrada;
                noviRed["Drzava"] = ImeDrzave;
                if(prosjekCheck.Count() != 0)
                {
                    var prosjek = db.PolozeniPredmeti.Where(x => x.StudentId == studenti[i].Id).Average(x => x.Ocjena);
                    noviRed["Prosjek"] = prosjek;
                }
                else
                {
                    noviRed["Prosjek"] = 5; //messy af popravi ovo kasnije
                }

                dsStudenti.Rows.Add(noviRed);
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dsStudenti;
        }
    }
}
