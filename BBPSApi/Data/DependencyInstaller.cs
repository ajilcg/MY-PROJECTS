using BBPSApi.Interface;
using BBPSApi.Service;
using Microsoft.EntityFrameworkCore;
using NGLMoSy.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace BBPSApi.Data
{
    public class DependencyInstaller
    {
        public static void InjectDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<AppSettings>(options => configuration.GetSection("Jwt"));
            serviceCollection.AddOptions();
            serviceCollection.AddMemoryCache();

            InjectDependenciesForDAL(serviceCollection, configuration);
            InjectDependenciesForBL(serviceCollection);

            //serviceCollection.AddTransient<ICacheProvider, InMemoryCache>();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void InjectDependenciesForDAL(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //var dbConnectionString = configuration["Data:ConnectionStrings:DefaultConnection"];
            //serviceCollection.AddTransient<IDbConnection>((sp) => new OracleConnection(dbConnectionString));
            //serviceCollection
            //    .AddDbContext<ApplicationDBContext>(options => options.UseOracle(dbConnectionString));
            var key = Encoding.UTF8.GetBytes("kljsdkkdlo4454GG00155sajuklmbkdl");
            var iv = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");
            var dbConnection = configuration["Data:ApplicationDB:DefaultConnection"];
            var passwordencrypt = Convert.FromBase64String(configuration["Data:ApplicationDB:Password"]);
            var password = CommonEncryptDecrypt.DecryptStringFromBytes_Aes(passwordencrypt, key, iv);
            string dbConnectionString = dbConnection + password;
            serviceCollection.AddTransient<IDbConnection>((sp) => new OracleConnection(dbConnectionString));

            serviceCollection
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseOracle(dbConnectionString);
                options.EnableSensitiveDataLogging();
            });
        }

        private static void InjectDependenciesForBL(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddTransient<IUserService, UserService>();
            //serviceCollection.AddScoped<IJwtUtils, JwtUtils>();
            //serviceCollection.AddTransient<IReportService, ReportService>();
            serviceCollection.AddTransient<IDataService,DataService>();


        }


    }
}
