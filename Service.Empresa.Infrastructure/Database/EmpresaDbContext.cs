using Microsoft.EntityFrameworkCore;
using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Infrastructure.Database;

public class EmpresaDbContext : DbContext
{
    public DbSet<Domain.Entities.Empresa> Empresa => Set<Domain.Entities.Empresa>();
    public DbSet<Domain.Entities.RegimenTributario> RegimenTributario => Set<Domain.Entities.RegimenTributario>();
    public DbSet<Domain.Entities.EstadoContribuyente> EstadoContribuyente => Set<Domain.Entities.EstadoContribuyente>();
    public DbSet<Domain.Entities.CondicionContribuyente> CondicionContribuyente => Set<Domain.Entities.CondicionContribuyente>();
    public DbSet<Domain.Entities.CredencialSunat> CredencialSunat => Set<Domain.Entities.CredencialSunat>();

    public EmpresaDbContext(DbContextOptions<EmpresaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("empresa");

        modelBuilder.Entity<Domain.Entities.Empresa>(entity =>
        {
            entity.ToTable("empresa");
            entity.HasKey(e => e.IdEmpresa);
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.Ruc).HasColumnName("ruc").HasMaxLength(11).IsRequired();
            entity.HasIndex(e => new { e.Ruc, e.CreadoPor })
      .HasDatabaseName("ix_empresa_ruc_usuario_activo")
      .HasFilter("\"activo\" = true")
      .IsUnique();
            entity.Property(e => e.RazonSocial).HasColumnName("razon_social").HasMaxLength(255).IsRequired();
            entity.Property(e => e.NombreComercial).HasColumnName("nombre_comercial").HasMaxLength(255);
            entity.Property(e => e.CodigoRegimenTributario).HasColumnName("codigo_regimen_tributario").HasMaxLength(10).IsRequired();
            entity.Property(e => e.CodigoEstadoContribuyente).HasColumnName("codigo_estado_contribuyente").HasMaxLength(20).IsRequired();
            entity.Property(e => e.CodigoCondicionContribuyente).HasColumnName("codigo_condicion_contribuyente").HasMaxLength(20).IsRequired();
            entity.Property(e => e.DireccionFiscal).HasColumnName("direccion_fiscal").HasColumnType("text").IsRequired();
            entity.Property(e => e.Ubigeo).HasColumnName("ubigeo").HasMaxLength(6).IsRequired();
            entity.Property(e => e.MonedaBase).HasColumnName("moneda_base").HasMaxLength(3).HasDefaultValue("PEN");
            entity.Property(e => e.LogoUrl).HasColumnName("logo_url").HasColumnType("text");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").IsRequired();
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(150);
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
            entity.HasQueryFilter(e => e.Activo);
        });

        modelBuilder.Entity<Domain.Entities.RegimenTributario>(entity =>
        {
            entity.ToTable("regimen_tributario");
            entity.HasKey(e => e.Codigo);
            entity.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(10);
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(100);
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(150);
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
            entity.HasQueryFilter(e => e.Activo);
        });

        modelBuilder.Entity<Domain.Entities.EstadoContribuyente>(entity =>
        {
            entity.ToTable("estado_contribuyente");
            entity.HasKey(e => e.Codigo);
            entity.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(20);
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(100);
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(150);
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
            entity.HasQueryFilter(e => e.Activo);
        });

        modelBuilder.Entity<Domain.Entities.CondicionContribuyente>(entity =>
        {
            entity.ToTable("condicion_contribuyente");
            entity.HasKey(e => e.Codigo);
            entity.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(20);
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(100);
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(150);
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
            entity.HasQueryFilter(e => e.Activo);
        });

        modelBuilder.Entity<Domain.Entities.CredencialSunat>(entity =>
        {
            entity.ToTable("credencial_sunat");
            entity.HasKey(e => e.IdCredencial);
            entity.Property(e => e.IdCredencial).HasColumnName("id_credencial");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa").IsRequired();
            entity.Property(e => e.UsuarioSol).HasColumnName("usuario_sol").HasMaxLength(50).IsRequired();
            entity.Property(e => e.ClaveSol).HasColumnName("clave_sol").HasMaxLength(100).IsRequired();
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(150);
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
            entity.HasQueryFilter(e => e.Activo);
        });
    }
}