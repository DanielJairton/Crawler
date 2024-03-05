using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebScrapping.Models;

namespace WebScrapping.Data
{
    // Classe de contexto do banco de dados
    public class LogContext : DbContext
    {
        public DbSet<LOGROBO> LOGROBO { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Conexão casa
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-QKATFHK\\SQLEXPRESS; Database=WebScrapingDb;  Trusted_Connection=True; Integrated Security=True; TrustServerCertificate=true");

            //Conxão Senai
            //optionsBuilder.UseSqlServer("Data Source=PC03LAB2524\\SENAI; Database=WebScrapingDb;  User Id=sa; Password=senai.123");

            //Conexão Reginaldo
            
            optionsBuilder.UseSqlServer(
                @"Data Source=SQL9001.site4now.net;" +
                "Initial Catalog=db_aa5b20_apialmoxarifado;" +
                "User id=db_aa5b20_apialmoxarifado_admin;" +
                "Password=master@123"
            );

        }
    }
}
