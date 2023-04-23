using DAL.ConnectionFactory;
using DAL.IService;
using DAL.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;


namespace DAL.Service
{
    public class FundDALService : IFundDALService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public FundDALService(IDbConnectionFactory dbConnectionFactory)
        {
            _connectionFactory = dbConnectionFactory;
        }

        public int BulkInsertToMySQL(List<Fund> recordList)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            
            using (MySqlConnection mConnection = (MySqlConnection) _connectionFactory.CreateConnection())
            {
                var sCommand = BuildInsertSqlCommandString(recordList);
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    return myCmd.ExecuteNonQuery();
                }
            }
        }


        public (List<Fund>, string) SelectRecords(string CNPJ_FUNDO, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(CNPJ_FUNDO))
                {
                    return (new List<Fund>(), "The parameter CNPJ is mandatory");
                }
                List<Fund> selectedRecords = new List<Fund>();
                MySqlDataReader result;
                
                using (MySqlConnection mConnection = (MySqlConnection)_connectionFactory.CreateConnection())
                {

                    MySqlCommand selectCommand = BuildSelectSqlCommandString(CNPJ_FUNDO, startDate, endDate);
                    selectCommand.Connection = mConnection;
                    mConnection.Open();
                    result = selectCommand.ExecuteReader();
                    while (result.Read())
                    {
                        Fund row = new Fund()
                        {
                            Id = result.GetInt64(0),
                            CNPJ_FUNDO = result.GetString(1),
                            DT_COMPTC = result.GetDateTime(2).Date,
                            VL_TOTAL = result.GetDouble(3),
                            VL_QUOTA = result.GetString(4),
                            VL_PATRIM_LIQ = result.GetDouble(5),
                            CAPTC_DIA = result.GetDouble(6),
                            RESG_DIA = result.GetDouble(7),
                            NR_COTST = result.GetInt32(8)
                        };
                        selectedRecords.Add(row);
                    }
                    mConnection.Close();
                    return (selectedRecords,"");
                }
            }
            catch (Exception e)
            {
                return (new List<Fund>(), "Error when retrieving records from Database.");
            }
        }

        

        public StringBuilder BuildInsertSqlCommandString(List<Fund> recordList)
        {
            StringBuilder sCommand = new StringBuilder("INSERT INTO DAILY_INF (ID, CNPJ_FUNDO, DT_COMPTC, VL_TOTAL, VL_QUOTA, VL_PATRIM_LIQ, CAPTC_DIA, RESG_DIA, NR_COTST) VALUES ");
            List<string> Rows = new List<string>();
            foreach (Fund record in recordList)
            {
                Rows.Add(string.Format("('0','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                    MySqlHelper.EscapeString(record.CNPJ_FUNDO),
                    record.DT_COMPTC.Date.ToString("yyyy-MM-dd"),
                    record.VL_TOTAL.ToString(),
                    record.VL_QUOTA.ToString().Replace(".", string.Empty),
                    record.VL_PATRIM_LIQ,
                    record.CAPTC_DIA,
                    record.RESG_DIA,
                    record.NR_COTST));
            }
            sCommand.Append(string.Join(",", Rows));
            sCommand.Append(";");
            return sCommand;
        }

        public MySqlCommand BuildSelectSqlCommandString(string CNPJ_FUNDO, DateTime? startDate, DateTime? endDate)
        {
            
            MySqlCommand selectCommand;
            string selectCommandString = "SELECT * FROM daily_inf WHERE CNPJ_FUNDO = @CNPJ_FUNDOP";
            if (startDate.HasValue && startDate != DateTime.MinValue && endDate.HasValue && endDate != DateTime.MinValue)
            {
                selectCommandString += " AND DT_COMPTC BETWEEN @startDate AND @endDate ORDER BY DT_COMPTC ASC";
                selectCommand = new MySqlCommand(selectCommandString);
                selectCommand.Parameters.AddWithValue("@CNPJ_FUNDOP", CNPJ_FUNDO.ToString());
                selectCommand.Parameters.AddWithValue("@startDate", startDate.Value.Date.ToString("yyyy-MM-dd"));
                selectCommand.Parameters.AddWithValue("@endDate", endDate.Value.Date.ToString("yyyy-MM-dd"));
            }
            else if (startDate.HasValue && startDate != DateTime.MinValue)
            {
                selectCommandString += " AND DT_COMPTC > @startDate ORDER BY DT_COMPTC ASC";
                selectCommand = new MySqlCommand(selectCommandString);
                selectCommand.Parameters.AddWithValue("@CNPJ_FUNDOP", CNPJ_FUNDO);
                selectCommand.Parameters.AddWithValue("@startDate", startDate.Value.Date.ToString("yyyy-MM-dd"));
            }
            else if (endDate.HasValue && endDate != DateTime.MinValue)
            {
                selectCommandString += " AND DT_COMPTC < @endDate ORDER BY DT_COMPTC ASC";
                selectCommand = new MySqlCommand(selectCommandString);
                selectCommand.Parameters.AddWithValue("@CNPJ_FUNDOP", CNPJ_FUNDO);
                selectCommand.Parameters.AddWithValue("@endDate", endDate.Value.Date.ToString("yyyy-MM-dd"));
            }
            else
            {
                selectCommandString += " ORDER BY DT_COMPTC ASC";
                selectCommand = new MySqlCommand(selectCommandString);
                selectCommand.Parameters.Add(new MySqlParameter("@CNPJ_FUNDOP", CNPJ_FUNDO));
            }
            return selectCommand;
        }
    }
}
