using TictactoeBackend.Entity.Concretes;

namespace TictactoeBackend.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TictactoeDBContext : DbContext
    {
        // Your context has been configured to use a 'TictactoeDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'TictactoeBackend.Data.TictactoeDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TictactoeDBContext' 
        // connection string in the application configuration file.
        public TictactoeDBContext()
            : base("name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserStat> UserStats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure StudentId as PK for StudentAddress
            modelBuilder.Entity<UserStat>()
                .HasKey(t => t.UserId);

            // Configure StudentId as FK for StudentAddress
            modelBuilder.Entity<User>()
                .HasOptional(t => t.Stats)
                .WithRequired(st => st.User);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}