using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DAL.Model;

namespace DAL
{
    public partial class DBContext : IdentityDbContext<User>
    {
        #region Constructor
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { }
        #endregion

        public virtual DbSet<Film> Film { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Participation> Participation { get; set; }
        public virtual DbSet<Studio> Studio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasMany(e => e.Films)
                    .WithMany(e => e.Genres);
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasMany(e => e.Comments)
                    .WithOne(e => e.Film)
                    .HasForeignKey(e => e.FilmId);

                entity.HasMany(e => e.RateCollection)
                    .WithOne(e => e.Film)
                    .HasForeignKey(e => e.FilmId);

                entity.HasMany(e => e.InFavorites)
                    .WithOne(e => e.Film)
                    .HasForeignKey(e => e.FilmId);

                entity.HasMany(e => e.Persons)
                    .WithOne(e => e.Film)
                    .HasForeignKey(e => e.FilmId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.Comments)
                    .WithOne(e => e.Author)
                    .HasForeignKey(e => e.AuthorId);

                entity.HasMany(e => e.RateCollection)
                    .WithOne(e => e.Author)
                    .HasForeignKey(e => e.AuthorId);

                entity.HasMany(e => e.Favorites)
                    .WithOne(e => e.Author)
                    .HasForeignKey(e => e.AuthorId);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasMany(e => e.Participations)
                    .WithOne(e => e.Person)
                    .HasForeignKey(e => e.PersonId);
            });

            modelBuilder.Entity<Studio>(entity =>
            {
                entity.HasMany(e => e.Films)
                    .WithOne(e => e.Studio)
                    .HasForeignKey(e => e.StudioId);
            });
        }
    }
}
