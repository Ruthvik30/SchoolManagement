﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

[Table("Attendance")]
public partial class Attendance
{
    [Key]
    [Column("AttendanceID")]
    public int AttendanceId { get; set; }

    [Column("StudentID")]
    public int? StudentId { get; set; }

    [Column("ClassID")]
    public int? ClassId { get; set; }

    [Column(TypeName = "date")]
    public DateTime AttendanceDate { get; set; }

    public bool IsPresent { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Attendances")]
    public virtual Class? Class { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Attendances")]
    public virtual StudentDetail? Student { get; set; }
}
