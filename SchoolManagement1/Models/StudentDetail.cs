using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

public partial class StudentDetail
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [StringLength(50)]
    public string? FatherName { get; set; }

    [StringLength(15)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [Column("ClassID")]
    public int? ClassId { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("ClassId")]
    [InverseProperty("StudentDetails")]
    public virtual Class? Class { get; set; }

    [InverseProperty("Student")]
    public virtual StudentTransportation? StudentTransportation { get; set; }
    public string DOB()
    {
        return DateOfBirth.ToString().Substring(0, 10) ?? "Date of Birth";
    }
}
