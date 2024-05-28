﻿using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Models
{
    public class HomeBankingContext : DbContext
    {
        public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }


        //dbsets
        public DbSet<Client> Clients { get; set; }
    }
}
