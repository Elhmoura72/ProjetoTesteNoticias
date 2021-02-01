using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjetoNoticias.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAutor> TbAutor { get; set; }
        public virtual DbSet<TbCategoria> TbCategoria { get; set; }
        public virtual DbSet<TbLogin> TbLogin { get; set; }
        public virtual DbSet<TbNoticias> TbNoticias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=dbNoticias;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbAutor>(entity =>
            {
                entity.ToTable("tbAutor");
            });

            modelBuilder.Entity<TbCategoria>(entity =>
            {
                entity.ToTable("tbCategoria");
            });

            modelBuilder.Entity<TbLogin>(entity =>
            {
                entity.ToTable("tbLogin");
            });

            modelBuilder.Entity<TbNoticias>(entity =>
            {
                entity.ToTable("tbNoticias");

                entity.Property(e => e.AutorId).HasColumnName("Autor_Id");

                entity.Property(e => e.CategoriaId).HasColumnName("Categoria_Id");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Titulo).HasMaxLength(50);

                entity.HasOne(d => d.Autor)
                    .WithMany(p => p.TbNoticias)
                    .HasForeignKey(d => d.AutorId)
                    .HasConstraintName("FK_tbNoticias_tbAutor");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.TbNoticias)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK_tbNoticias_tbCategoria");
            });
        }
    }
}
