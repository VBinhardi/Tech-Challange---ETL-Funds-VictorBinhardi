using ETL_FUNDS;
using MySqlConnector;
using System.Configuration;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateMySQLDatabaseOnStartup();
        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup(builder.Configuration);

        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        startup.Configure(app, builder.Environment);

        app.Run();


        
    }
    static void CreateMySQLDatabaseOnStartup()
    {
        MySqlConnection conn = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mySqlConnectionString"].ConnectionString);

        conn.Open();

        MySqlCommand cmd = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `DAILY_INF` 
            (ID BIGINT NOT NULL AUTO_INCREMENT,
            CNPJ_FUNDO VARCHAR(255),
            DT_COMPTC date,
            VL_TOTAL double,
            VL_QUOTA VARCHAR(255),
            VL_PATRIM_LIQ double,
            CAPTC_DIA double,
            RESG_DIA double,
            NR_COTST int,
            PRIMARY KEY(ID)
            )", conn);

        cmd.ExecuteNonQuery();
        conn.Close();
    }
}