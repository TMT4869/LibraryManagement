using FA.LibraryManagement.Core.Data;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FA.LibraryManagement.Core.Context;

/// <summary>
///     The just blog context class
/// </summary>
/// <seealso cref="DbContext" />
public class
    LibraryManagementContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LibraryManagementContext" /> class
    /// </summary>
    public LibraryManagementContext()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LibraryManagementContext" /> class
    /// </summary>
    /// <param name="options">The options</param>
    public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Gets or sets the value of the comments
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user claims
    /// </summary>
    public DbSet<UserClaim> UserClaims { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user logins
    /// </summary>
    public DbSet<UserLogin> UserLogins { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user tokens
    /// </summary>
    public DbSet<UserToken> UserTokens { get; set; }

    /// <summary>
    ///     Gets or sets the value of the roles
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    ///     Gets or sets the value of the role claims
    /// </summary>
    public DbSet<RoleClaim> RoleClaims { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user roles
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookImage> BookImages { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
    public DbSet<BorrowingDetail> BorrowingDetails { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<History> Histories { get; set; }


    /// <summary>
    ///     Ons the configuring using the specified options builder
    /// </summary>
    /// <param name="optionsBuilder">The options builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var buider = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);

        var configuration = buider.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);
    }


    /// <summary>
    ///     Ons the model creating using the specified model builder
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b => { b.ToTable("Users"); });

        modelBuilder.Entity<UserClaim>(b => { b.ToTable("UserClaims"); });

        modelBuilder.Entity<UserLogin>(b => { b.ToTable("UserLogins"); });

        modelBuilder.Entity<UserToken>(b => { b.ToTable("UserTokens"); });

        modelBuilder.Entity<Role>(b => { b.ToTable("Roles"); });

        modelBuilder.Entity<RoleClaim>(b => { b.ToTable("RoleClaims"); });

        modelBuilder.Entity<UserRole>(b => { b.ToTable("UserRoles"); });

        modelBuilder.Entity<User>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<UserLogin>(b => { b.HasKey(l => new { l.LoginProvider, l.ProviderKey }); });

        modelBuilder.Entity<Role>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId);

        modelBuilder.Entity<BookImage>()
            .HasOne(bi => bi.Book)
            .WithMany(b => b.BookImages)
            .HasForeignKey(bi => bi.BookId);

        modelBuilder.Entity<History>()
            .HasOne(h => h.User)
            .WithMany(u => u.Histories)
            .HasForeignKey(h => h.UserId);

        modelBuilder.Entity<History>()
            .HasOne(h => h.Book)
            .WithMany(b => b.Histories)
            .HasForeignKey(h => h.BookId);

        modelBuilder.Entity<Borrowing>()
            .HasOne(o => o.User)
            .WithMany(u => u.Borrowings)
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<BorrowingDetail>()
            .HasOne(od => od.Borrowing)
            .WithMany(o => o.BorrowingDetails)
            .HasForeignKey(od => od.BorrowingId);

        modelBuilder.Entity<BorrowingDetail>()
            .HasOne(od => od.Book)
            .WithMany(b => b.BorrowingDetails)
            .HasForeignKey(bd => bd.BookId);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Book)
            .WithMany(b => b.Carts)
            .HasForeignKey(c => c.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Seed();
    }
}