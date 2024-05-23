using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace API_UG.Models;

public partial class GidromashContext : DbContext
{
    public GidromashContext()
    {
    }

    public static GidromashContext _context { get; set; }
    public static GidromashContext GetContext()
    {
        if (_context == null)
            _context = new GidromashContext();
        return _context;
    }

    public GidromashContext(DbContextOptions<GidromashContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agreement> Agreements { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Productpart> Productparts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Servicepart> Serviceparts { get; set; }

    public virtual DbSet<Statusemployee> Statusemployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=huisosibidlo;database=gidromash", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Agreement>(entity =>
        {
            entity.HasKey(e => e.AgreementId).HasName("PRIMARY");

            entity.ToTable("agreement");

            entity.HasIndex(e => e.AgreementCompany, "comp_idx");

            entity.HasIndex(e => e.AgreementEmployee, "empl_idx");

            entity.Property(e => e.AgreementId)
                .ValueGeneratedNever()
                .HasColumnName("AgreementID");

            entity.HasOne(d => d.AgreementCompanyNavigation).WithMany(p => p.Agreements)
                .HasForeignKey(d => d.AgreementCompany)
                .HasConstraintName("comp");

            entity.HasOne(d => d.AgreementEmployeeNavigation).WithMany(p => p.Agreements)
                .HasForeignKey(d => d.AgreementEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empl");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PRIMARY");

            entity.ToTable("company");

            entity.HasIndex(e => e.CompanyId, "ClientID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.CompanyInn, "CompanyINN_UNIQUE").IsUnique();

            entity.HasIndex(e => e.CompanyLogin, "CompanyLogin_UNIQUE").IsUnique();

            entity.HasIndex(e => e.CompanyName, "CompanyName_UNIQUE").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyAddress).HasColumnType("text");
            entity.Property(e => e.CompanyDelegate).HasMaxLength(200);
            entity.Property(e => e.CompanyEmailAddress).HasMaxLength(55);
            entity.Property(e => e.CompanyInn)
                .HasMaxLength(12)
                .HasColumnName("CompanyINN");
            entity.Property(e => e.CompanyLogin).HasMaxLength(20);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.CompanyPassword).HasMaxLength(20);
            entity.Property(e => e.CompanyPhone).HasMaxLength(20);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.EmployeeId, "EmployeeID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.EmployeeLogin, "EmployeeLogin_UNIQUE").IsUnique();

            entity.HasIndex(e => e.EmployeePassport, "EmployeePassport_UNIQUE").IsUnique();

            entity.HasIndex(e => e.EmployeePhone, "EmployeePhone_UNIQUE").IsUnique();

            entity.HasIndex(e => e.EmployeePost, "post_idx");

            entity.HasIndex(e => e.EmployeeStatus, "status_idx");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeFirstName).HasMaxLength(45);
            entity.Property(e => e.EmployeeLogin).HasMaxLength(20);
            entity.Property(e => e.EmployeeName).HasMaxLength(45);
            entity.Property(e => e.EmployeePassport).HasMaxLength(20);
            entity.Property(e => e.EmployeePassword).HasMaxLength(20);
            entity.Property(e => e.EmployeePatronymic).HasMaxLength(45);
            entity.Property(e => e.EmployeePhone).HasMaxLength(20);

            entity.HasOne(d => d.EmployeePostNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeePost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post");

            entity.HasOne(d => d.EmployeeStatusNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("status");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.OrderId, "OrderID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.OrderDate, "orderprod_idx");

            entity.HasIndex(e => e.OrderCompany, "ordeservice_idx");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.OrderCompanyNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cm");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("post");

            entity.HasIndex(e => e.PostId, "PostID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PostName, "PostName_UNIQUE").IsUnique();

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.PostName).HasMaxLength(45);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticle).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductArticle, "ProductID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.ProductName, "ProductName_UNIQUE").IsUnique();

            entity.HasIndex(e => e.ProductCategory, "category_idx");

            entity.Property(e => e.ProductDescription).HasColumnType("text");
            entity.Property(e => e.ProductName).HasMaxLength(100);

            entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("category");
        });

        modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PRIMARY");

            entity.ToTable("productcategory");

            entity.HasIndex(e => e.ProductCategoryId, "CategoryID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.ProductCategoryName, "CategoryName_UNIQUE").IsUnique();

            entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");
            entity.Property(e => e.ProductCategoryName).HasMaxLength(45);
        });

        modelBuilder.Entity<Productpart>(entity =>
        {
            entity.HasKey(e => e.ProductPartId).HasName("PRIMARY");

            entity.ToTable("productpart");

            entity.HasIndex(e => e.AgreementId, "companyid_idx");

            entity.HasIndex(e => e.OrderId, "orderid_idx");

            entity.HasIndex(e => e.ProductArticle, "productid_idx");

            entity.Property(e => e.ProductPartId).HasColumnName("ProductPartID");
            entity.Property(e => e.AgreementId).HasColumnName("AgreementID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.Agreement).WithMany(p => p.Productparts)
                .HasForeignKey(d => d.AgreementId)
                .HasConstraintName("companyid");

            entity.HasOne(d => d.Order).WithMany(p => p.Productparts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("orderid");

            entity.HasOne(d => d.ProductArticleNavigation).WithMany(p => p.Productparts)
                .HasForeignKey(d => d.ProductArticle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productid");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("service");

            entity.HasIndex(e => e.ServiceId, "ServiceID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.ServiceName, "ServiceName_UNIQUE").IsUnique();

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ServiceDescription).HasColumnType("text");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<Servicepart>(entity =>
        {
            entity.HasKey(e => e.ServicePartId).HasName("PRIMARY");

            entity.ToTable("servicepart");

            entity.HasIndex(e => e.ServicePartId, "ServicePartID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.AgreementId, "company_idx");

            entity.HasIndex(e => e.OrderId, "order_idx");

            entity.HasIndex(e => e.ServiceId, "service_idx");

            entity.Property(e => e.ServicePartId).HasColumnName("ServicePartID");
            entity.Property(e => e.AgreementId).HasColumnName("AgreementID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Agreement).WithMany(p => p.Serviceparts)
                .HasForeignKey(d => d.AgreementId)
                .HasConstraintName("company");

            entity.HasOne(d => d.Order).WithMany(p => p.Serviceparts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order");

            entity.HasOne(d => d.Service).WithMany(p => p.Serviceparts)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service");
        });

        modelBuilder.Entity<Statusemployee>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("statusemployee");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(45);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
