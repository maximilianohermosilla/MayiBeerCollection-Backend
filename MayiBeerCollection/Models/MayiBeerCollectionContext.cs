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

    public virtual DbSet<Archivo> Archivos { get; set; }

    public virtual DbSet<ArchivoFilestream> ArchivoFilestreams { get; set; }

    public virtual DbSet<Cerveza> Cervezas { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<Estilo> Estilos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost; Database=MayiBeerCollection; Trusted_Connection=True; TrustServerCertificate=True");
        //=> optionsBuilder.UseSqlServer("Data Source=SQL5097.site4now.net;Initial Catalog=db_a934ba_mayibeercollection;User Id=db_a934ba_mayibeercollection_admin;Password=Caslacapo1908**");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Archivo>(entity =>
        {
            entity.ToTable("Archivo");

            entity.Property(e => e.Archivo1).HasColumnName("Archivo");
        });

        modelBuilder.Entity<ArchivoFilestream>(entity =>
        {
            entity.ToTable("ArchivoFilestream");

            entity.HasIndex(e => e.Idguid, "UQ__ArchivoF__D1B7D08051915287").IsUnique();

            entity.Property(e => e.FileAttribute)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FileCreateDate).HasColumnType("datetime");
            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.FileSize).HasColumnType("numeric(10, 5)");
            entity.Property(e => e.Idguid).HasColumnName("IDGUID");
            entity.Property(e => e.RootDirectory).IsUnicode(false);
        });

        modelBuilder.Entity<Cerveza>(entity =>
        {
            entity.ToTable("Cerveza");

            entity.Property(e => e.Ibu).HasColumnName("IBU");
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Cervezas)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Cerveza_Archivo");

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

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Estilos)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Estilo_Archivo");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("Marca");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Marcas)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Marca_Archivo");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Pais)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Pais_Archivo");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.ToTable("Perfil");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdPerfil)
                .HasConstraintName("FK_Usuario_Perfil");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
