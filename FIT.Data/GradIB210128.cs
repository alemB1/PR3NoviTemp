using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Data
{
    public class GradIB210128
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Status { get; set; }
        public int DrzavaId { get; set; }
        public DrzavaIB210128 Drzava { get; set; }
        public override string ToString()
        {
            return Naziv.ToString();
        }
    }
}
