using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

public partial class SchoolManagementContext : DbContext
{
    public SchoolManagementContext()
    {
    }

    public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<FeeStructure> FeeStructures { get; set; }

    public virtual DbSet<StudentDetail> StudentDetails { get; set; }

    public virtual DbSet<StudentTransportation> StudentTransportations { get; set; }

    public virtual DbSet<Transportation> Transportations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69263C41D17A15");

            entity.HasOne(d => d.Class).WithMany(p => p.Attendances).HasConstraintName("FK__Attendanc__Class__6477ECF3");

            entity.HasOne(d => d.Student).WithMany(p => p.Attendances).HasConstraintName("FK__Attendanc__Stude__6383C8BA");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927A094FCE930");
        });

        modelBuilder.Entity<FeeStructure>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__FeeStruc__CB1927A07375A94E");

            entity.Property(e => e.ClassId).ValueGeneratedNever();
            entity.Property(e => e.FeeId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Class).WithOne(p => p.FeeStructure).HasConstraintName("FK__FeeStruct__Class__59FA5E80");
        });

        modelBuilder.Entity<StudentDetail>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__StudentD__32C52A797C0DACEC");

            entity.HasOne(d => d.Class).WithMany(p => p.StudentDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__StudentDe__Class__571DF1D5");
        });

        modelBuilder.Entity<StudentTransportation>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__StudentT__32C52A797D16289E");

            entity.Property(e => e.StudentId).ValueGeneratedNever();

            entity.HasOne(d => d.Student).WithOne(p => p.StudentTransportation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTr__Stude__5EBF139D");

            entity.HasOne(d => d.Transportation).WithMany(p => p.StudentTransportations).HasConstraintName("FK__StudentTr__Trans__5FB337D6");
        });

        modelBuilder.Entity<Transportation>(entity =>
        {
            entity.HasKey(e => e.TransportationId).HasName("PK__Transpor__87E47956711F527B");

            entity.Property(e => e.Amount).HasDefaultValueSql("((0.00))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
