using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

public partial class Class
{
    [Key]
    [Column("ClassID")]
    public int ClassId { get; set; }

    [StringLength(20)]
    public string ClassName { get; set; } = null!;

    [InverseProperty("Class")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [InverseProperty("Class")]
    public virtual FeeStructure? FeeStructure { get; set; }

    [InverseProperty("Class")]
    public virtual ICollection<StudentDetail> StudentDetails { get; set; } = new List<StudentDetail>();
}
