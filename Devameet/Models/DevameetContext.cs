﻿using Microsoft.EntityFrameworkCore;

namespace Devameet.Models
{
    public class DevameetContext : DbContext
    {
        public DevameetContext(DbContextOptions<DevameetContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } //Instacia para criar/ acessar o Model Users no Banco de Dados
    }
}
