using Bogus;
using DAL.Model;
using DAL.Tests.FakerDataGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Tests.FakeDataGenerator
{
    public class FakeFund
    { 
        public Fund getFakeFund()
        {
            var faker = new Faker<Fund>()
                .RuleFor(u => u.CNPJ_FUNDO, f => "00.017.024/0001-53")
                .RuleFor(u => u.DT_COMPTC, f => f.Date.Between(DateTime.Today.AddYears(-3).Date, DateTime.Today))
                .RuleFor(u => u.VL_TOTAL, f => f.Random.Double(1, 10000000))
                .RuleFor(u => u.VL_QUOTA, f => f.Random.UInt().ToString())
                .RuleFor(u => u.VL_PATRIM_LIQ, f => f.Random.Double(1, 1000000))
                .RuleFor(u => u.CAPTC_DIA, f => 0)
                .RuleFor(u => u.RESG_DIA, f => f.Random.Double(1, 1000000))
                .RuleFor(u => u.NR_COTST, f => 1);

            return faker;
        }

        public List<Fund> getFakeFundList()
        {
            List<Fund> list = new List<Fund>();
            for(int i  = 0; i <= 2; i++)
            {
                var faker = getFakeFund();
                list.Add(faker);
            }
            return list;
        }
    }
}
