using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChurchApp.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Church> Churches { get; set; } = null!;
        public virtual DbSet<Donation> Donations { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Profil> Profils { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= localhost;Database=churchdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Church>(entity =>
            {
                entity.HasKey(e => e.IdChurch);

                entity.ToTable("CHURCH");

                entity.Property(e => e.IdChurch)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CHURCH");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.ChurchName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CHURCH_NAME");
            });

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.HasKey(e => e.IdDonation);

                entity.ToTable("DONATION");

                entity.HasIndex(e => e.IdChurch, "R_DONATION_CHURCH_FK");

                entity.HasIndex(e => e.IdEvent, "R_DONATION_EVENT_FK");

                entity.HasIndex(e => e.IdMember, "R_DONATION_MEMBER_FK");

                entity.Property(e => e.IdDonation)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_DONATION");

                entity.Property(e => e.DateDonation)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE_DONATION");

                entity.Property(e => e.IdChurch)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_CHURCH");

                entity.Property(e => e.IdEvent)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_EVENT");

                entity.Property(e => e.IdMember)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MEMBER");

                entity.Property(e => e.Observation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVATION");

                entity.Property(e => e.Price)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("PRICE");

                entity.HasOne(d => d.IdChurchNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.IdChurch)
                    .HasConstraintName("FK_DONATION_R_DONATIO_CHURCH");

                entity.HasOne(d => d.IdEventNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.IdEvent)
                    .HasConstraintName("FK_DONATION_EVENT");

                entity.HasOne(d => d.IdMemberNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.IdMember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DONATION_MEMBER");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.IdEvent);

                entity.ToTable("EVENT");

                entity.Property(e => e.IdEvent)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_EVENT");

                entity.Property(e => e.LabelEvent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LABEL_EVENT");

                entity.Property(e => e.Observation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVATION");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.IdMember);

                entity.ToTable("MEMBER");

                entity.HasIndex(e => e.IdChurch, "R_CHURCH_MEMBER_FK");

                entity.Property(e => e.IdMember)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_MEMBER");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.IdChurch)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_CHURCH");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");

                entity.HasOne(d => d.IdChurchNavigation)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.IdChurch)
                    .HasConstraintName("FK_MEMBER_R_CHURCH__CHURCH");
            });

            modelBuilder.Entity<Profil>(entity =>
            {
                entity.HasKey(e => e.IdProfil);

                entity.ToTable("PROFIL");

                entity.Property(e => e.IdProfil)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_PROFIL");

                entity.Property(e => e.Label)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LABEL");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("USER");

                entity.HasIndex(e => e.IdMember, "R_MEMBER_USER_FK");

                entity.HasIndex(e => e.IdChurch, "R_USER_CHERCH_FK");

                entity.Property(e => e.IdUser)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_USER");

                entity.Property(e => e.IdChurch)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_CHURCH");

                entity.Property(e => e.IdMember)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID_MEMBER");

                entity.Property(e => e.IsDefault).HasColumnName("IS_DEFAULT");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");

                entity.HasOne(d => d.IdChurchNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdChurch)
                    .HasConstraintName("FK_USER_R_USER_CH_CHURCH");

                entity.HasOne(d => d.IdMemberNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdMember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_MEMBER");

                entity.HasMany(d => d.IdProfils)
                    .WithMany(p => p.IdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "RUserPorfil",
                        l => l.HasOne<Profil>().WithMany().HasForeignKey("IdProfil").HasConstraintName("FK_R_USER_P_R_USER_PO_PROFIL"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("IdUser").HasConstraintName("FK_R_USER_P_R_USER_PO_USER"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdProfil");

                            j.ToTable("R_USER_PORFIL");

                            j.HasIndex(new[] { "IdProfil" }, "R_USER_PORFIL2_FK");

                            j.HasIndex(new[] { "IdUser" }, "R_USER_PORFIL_FK");

                            j.IndexerProperty<decimal>("IdUser").HasColumnType("numeric(10, 0)").HasColumnName("ID_USER");

                            j.IndexerProperty<decimal>("IdProfil").HasColumnType("numeric(10, 0)").HasColumnName("ID_PROFIL");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
