using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_infra_data.Context;

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

                options.UseMySQL($"Server={server};Port={port};Database={database};Uid={username};Pwd={password}", opt =>
                {
                    opt.CommandTimeout(180);
                    //opt.EnableRetryOnFailure(5);
                });
            });
        }
    }
}
