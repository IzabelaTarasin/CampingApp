using System;
using CampingApp_Server.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampingApp_Server.Database
{
    public class ApplicationDbContext : IdentityDbContext<User> //IdentityDbContext odpowieada za dodanie uzytkownika
    {
        public DbSet<Place> Places { get; set; } //dzieki temu utworzy tabele i polaczenie
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //wiazemy role uzytkownikow
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityUserClaim<string>>().ToTable("RoleClaims");

            //tworzenie ról
            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Id = "1", //tu wyjatkowo id jest string
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Standard",
                    ConcurrencyStamp = "2",
                    NormalizedName = "STANDARD"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Business",
                    ConcurrencyStamp = "3",
                    NormalizedName = "BUSINESS"
                });

            //tworzenie statusów  //predifiniowane w bazie
            builder.Entity<Status>()
                .HasData(
                new Status
                {
                    Id = 1,
                    StatusName = "Aktywna"
                },
                new Status
                {
                    Id = 2,
                    StatusName = "Anulowana"
                },
                new Status
                {
                    Id = 3,
                    StatusName = "Zrealizowana"
                }
                );
        }
    }
}