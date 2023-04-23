using DAL.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IFundDALService
    {
        public int BulkInsertToMySQL(List<Fund> recordList);
        public (List<Fund>, string) SelectRecords(string CNPJ_FUNDO, DateTime? startDate, DateTime? endDate);
        public StringBuilder BuildInsertSqlCommandString(List<Fund> recordList);
        public MySqlCommand BuildSelectSqlCommandString(string CNPJ_FUNDO, DateTime? startDate, DateTime? endDate);
    }
}
