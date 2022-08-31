using Microsoft.EntityFrameworkCore;
using Shopping.Data.Entities;

namespace Shopping.Data
{
    //Esta clase hereda del Dbcontexto mediante un paquete que se llama EntityFrameworkCore
    public class DataContext: DbContext
    {

        //Para conexión a la DB, en el constructor establezco una clase generica de tipo DbcontextOption, de tipo Dtacontext llamda options
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

    //Por cada entidad que se mapee se debe crear una porp de tipo dbset para mapear, aca se pluraliza en ingles
        public DbSet<Country>Countries { get; set; }


        //Crear una entidad
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();    
        }

    }
}
