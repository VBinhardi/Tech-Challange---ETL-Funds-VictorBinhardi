using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Fund2
    {
        [Ignore]
        public long Id { get; set; }
        public string CNPJ_FUNDO { get; set; }
        public DateTime DT_COMPTC { get; set; }
        public string VL_TOTAL { get; set; }
        public string VL_QUOTA { get; set; }
        public string VL_PATRIM_LIQ { get; set; }
        public string CAPTC_DIA { get; set; }
        public string RESG_DIA { get; set; }
        public string NR_COTST { get; set; }
    }
}
