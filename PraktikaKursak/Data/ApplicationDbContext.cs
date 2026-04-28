using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using практы_курсак.Models;

namespace практы_курсак.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Включить детальное логирование для отладки
        this.ChangeTracker.Tracked += OnEntityTracked;
        this.ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    private void OnEntityTracked(object? sender, EntityTrackedEventArgs e)
    {
        if (e.Entry.Entity is Client client && e.Entry.State == EntityState.Added)
        {
            client.RegisteredAt = DateTime.UtcNow;
        }
        if (e.Entry.Entity is Booking booking && e.Entry.State == EntityState.Added)
        {
            booking.CreatedAt = DateTime.UtcNow;
        }
    }

    private void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        // Можно добавить логирование при необходимости
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка таблицы Services
        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Services");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            entity.Property(e => e.DurationMinutes).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Настройка таблицы Clients
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Phone).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.RegisteredAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Настройка таблицы Bookings
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(e => e.BookingDate).HasColumnType("date");
            entity.Property(e => e.BookingTime).HasColumnType("time without time zone");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Уникальное ограничение
            entity.HasIndex(e => new { e.BookingDate, e.BookingTime }).IsUnique();

            // Связи
            entity.HasOne(e => e.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // Переопределяем SaveChanges для автоматической конвертации DateTime в UTC
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Booking || e.Entity is Client)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is Client client)
                {
                    client.RegisteredAt = DateTime.UtcNow;
                }
                if (entry.Entity is Booking booking)
                {
                    booking.CreatedAt = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}