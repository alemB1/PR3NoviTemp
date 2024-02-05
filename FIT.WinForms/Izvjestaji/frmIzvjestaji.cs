using FIT.Data;
using FIT.Infrastructure;
using Microsoft.Reporting.WinForms;

namespace FIT.WinForms.Izvjestaji
{
    public partial class frmIzvjestaji : Form
    {
        private DrzavaIB210128 drzavaIB210128;
        DLWMSDbContext db = new DLWMSDbContext();
        public frmIzvjestaji()
        {
        }

        public frmIzvjestaji(DrzavaIB210128 drzavaIB210128)
        {
            InitializeComponent();
            this.drzavaIB210128 = drzavaIB210128;
        }

        private void frmIzvjestaji_Load(object sender, EventArgs e)
        {
            var rpc = new ReportParameterCollection();
            rpc.Add(new ReportParameter("Rb", drzavaIB210128.Id.ToString()));
            rpc.Add(new ReportParameter("Grad",db.GradoviIB210128.FirstOrDefault(x => x.DrzavaId == drzavaIB210128.Id).Naziv.ToString())); //sranje
            rpc.Add(new ReportParameter("Drzava", drzavaIB210128.Naziv.ToString()));
            rpc.Add(new ReportParameter("Aktivan", drzavaIB210128.Status == 1 ? "DA" : "NE"));
            reportViewer1.LocalReport.SetParameters(rpc);
            reportViewer1.RefreshReport();
        }
    }
}
