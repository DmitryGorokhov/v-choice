using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace v_choice.Models
{
    public class DBContext: DbContext
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
        public virtual DbSet<Viewer> Viewer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Viewer>(entity =>
            {
                entity.HasMany(e => e.Favorites)
                .WithMany(e => e.Viewers);
            });
        }

    }
}
