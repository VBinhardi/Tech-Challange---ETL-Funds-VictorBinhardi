using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Fund
    {
        [Ignore]
        public long Id { get; set; }
        public string CNPJ_FUNDO { get; set; }
        public DateTime DT_COMPTC { get; set; }
        public double VL_TOTAL { get; set; }
        public string VL_QUOTA { get; set; }
        public double VL_PATRIM_LIQ { get; set; }
        public double CAPTC_DIA { get; set; }
        public double RESG_DIA { get; set; }
        public int NR_COTST { get; set; }
    }
}
