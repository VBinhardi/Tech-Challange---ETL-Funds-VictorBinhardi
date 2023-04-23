using CsvHelper.Configuration.Attributes;

namespace BLL.DTO
{
    public class CsvRowDTO
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
