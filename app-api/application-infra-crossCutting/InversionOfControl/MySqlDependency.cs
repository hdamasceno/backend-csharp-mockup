using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_infra_data.Context;
using MySql.Data.MySqlClient;

namespace application_infra_crossCutting.InversionOfControl
{
    public static class MySqlDependency
    {
        public static void AddMySqlDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MySqlContext>(options =>
            {
                var server = configuration["database:mysql:server"];
                var port = configuration["database:mysql:port"];
                var database = configuration["database:mysql:database"];
                var username = configuration["database:mysql:username"];
                var password = configuration["database:mysql:password"];


                var strConnection = configuration.GetConnectionString("MySQL");

                // dica valiosa :) pode passar aqui direto a string 
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(strConnection);

                //ou montar dinamicamente por exemplo, assim evita a string interpolation :)
                //$Server={server};Port=1234;Database=data;Uid=henry;Pwd=1234 blablablablaa
                builder = new MySqlConnectionStringBuilder
                {
                    Database = "data",
                    Port = 1234,
                    UserID = "henry"
                };

                options.UseMySQL(builder.ToString(), opt =>
                {
                    opt.CommandTimeout(180);
                    //opt.EnableRetryOnFailure(5);
                });
            });
        }
    }
}
