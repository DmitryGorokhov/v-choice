using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace v_choice.Models
{
    public partial class DBContext: IdentityDbContext<User>
    {
        #region Constructor
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        { }
        #endregion

        public virtual DbSet<Film> Film { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<User> User { get; set; }

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
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.Comments)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(e => e.Favorites)
                .WithMany(e => e.Users);
            });
        }

    }
}
