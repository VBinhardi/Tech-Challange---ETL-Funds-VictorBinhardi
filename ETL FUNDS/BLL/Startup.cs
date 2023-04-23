using BLL.Services;
using DAL.ConnectionFactory;
using DAL.IService;
using DAL.Service;
using DAL.Tests.FakeDataGenerator;
using DAL.Tests.FakerDataGenerator;

namespace ETL_FUNDS
{
    public class Startup
    {
        readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFundDALService, FundDALService>();
            services.AddScoped<IDbConnectionFactory, MySqlConnectionFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
