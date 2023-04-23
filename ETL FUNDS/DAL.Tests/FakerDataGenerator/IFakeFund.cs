using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Tests.FakerDataGenerator
{
    public interface IFakeFund
    {
        public Fund getFakeFund();
        public List<Fund> getFakeFundList();
    }
}
