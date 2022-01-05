using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using application_data_entities;

namespace application_infra_data.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<EmpresaModel>(new EmpresaModelMap().Configure);

            var entites = Assembly
                .Load("application_data_entities")
                .GetTypes()
                .Where(w => w.Namespace == "application_data_entities" && w.BaseType?.BaseType == typeof(Notifiable<Notification>));

            foreach (var item in entites)
            {
                modelBuilder.Entity(item).Ignore(nameof(Notifiable<Notification>.IsValid));                
                modelBuilder.Entity(item).Ignore(nameof(Notifiable<Notification>.Notifications));
            }
        }
    }
}
