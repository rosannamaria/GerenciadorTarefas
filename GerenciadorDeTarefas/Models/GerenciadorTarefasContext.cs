using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Models
{
    public class GerenciadorTarefasContext : DbContext
    {
        public GerenciadorTarefasContext(DbContextOptions<GerenciadorTarefasContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuario { get; set; }
        
    }
}
