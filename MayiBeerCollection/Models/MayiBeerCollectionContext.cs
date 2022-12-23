using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MayiBeerCollection.Models;

public partial class MayiBeerCollectionContext : DbContext
{
    public MayiBeerCollectionContext()
    {
    }

    public MayiBeerCollectionContext(DbContextOptions<MayiBeerCollectionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cerveza> Cervezas { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<Estilo> Estilos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost; Database=MayiBeerCollection; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cerveza>(entity =>
        {
            entity.ToTable("Cerveza");

            entity.Property(e => e.Ibu).HasColumnName("IBU");
            entity.Property(e => e.Imagen)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Cervezas)
                .HasForeignKey(d => d.IdCiudad)
                .HasConstraintName("FK_Cerveza_Ciudad");

            entity.HasOne(d => d.IdEstiloNavigation).WithMany(p => p.Cervezas)
                .HasForeignKey(d => d.IdEstilo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cerveza_Estilo");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Cervezas)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cerveza_Marca");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.ToTable("Ciudad");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Ciudads)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudad_Pais");
        });

        modelBuilder.Entity<Estilo>(entity =>
        {
            entity.ToTable("Estilo");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("Marca");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
