using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MayiBeerCollection.Models;

public partial class DbA934baMayibeercollectionContext : DbContext
{
    public DbA934baMayibeercollectionContext()
    {
    }

    public DbA934baMayibeercollectionContext(DbContextOptions<DbA934baMayibeercollectionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acto> Acto { get; set; }

    public virtual DbSet<Archivo> Archivo { get; set; }

    public virtual DbSet<ArchivoFilestream> ArchivoFilestream { get; set; }

    public virtual DbSet<Caratula> Caratula { get; set; }

    public virtual DbSet<Cerveza> Cerveza { get; set; }

    public virtual DbSet<Ciudad> Ciudad { get; set; }

    public virtual DbSet<Estilo> Estilo { get; set; }

    public virtual DbSet<Expediente> Expediente { get; set; }

    public virtual DbSet<Log> Log { get; set; }

    public virtual DbSet<Marca> Marca { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<Perfil> Perfil { get; set; }

    public virtual DbSet<SituacionRevista> SituacionRevista { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SQL5097.site4now.net;Initial Catalog=db_a934ba_mayibeercollection;User Id=db_a934ba_mayibeercollection_admin;Password=Caslacapo1908**");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Acto>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Archivo>(entity =>
        {
            entity.Property(e => e.Archivo1).HasColumnName("Archivo");
        });

        modelBuilder.Entity<ArchivoFilestream>(entity =>
        {
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

        modelBuilder.Entity<Caratula>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cerveza>(entity =>
        {
            entity.Property(e => e.Ibu).HasColumnName("IBU");
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones).IsUnicode(false);

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Cerveza)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Cerveza_Archivo");

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Cerveza)
                .HasForeignKey(d => d.IdCiudad)
                .HasConstraintName("FK_Cerveza_Ciudad");

            entity.HasOne(d => d.IdEstiloNavigation).WithMany(p => p.Cerveza)
                .HasForeignKey(d => d.IdEstilo)
                .HasConstraintName("FK_Cerveza_Estilo");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Cerveza)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cerveza_Marca");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Ciudad)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudad_Pais");
        });

        modelBuilder.Entity<Estilo>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Estilo)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Estilo_Archivo");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Expediente1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Expediente");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaExpediente).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones).IsUnicode(false);

            entity.HasOne(d => d.IdActoNavigation).WithMany(p => p.Expediente)
                .HasForeignKey(d => d.IdActo)
                .HasConstraintName("FK_Expediente_Acto");

            entity.HasOne(d => d.IdCaratulaNavigation).WithMany(p => p.Expediente)
                .HasForeignKey(d => d.IdCaratula)
                .HasConstraintName("FK_Expediente_Caratula");

            entity.HasOne(d => d.IdSituacionRevistaNavigation).WithMany(p => p.Expediente)
                .HasForeignKey(d => d.IdSituacionRevista)
                .HasConstraintName("FK_Expediente_SituacionRevista");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArchivoNavigation).WithMany(p => p.Marca)
                .HasForeignKey(d => d.IdArchivo)
                .HasConstraintName("FK_Marca_Archivo");
        });

        modelBuilder.Entity<Pais>(entity =>
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
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SituacionRevista>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
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

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdPerfil)
                .HasConstraintName("FK_Usuario_Perfil");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
