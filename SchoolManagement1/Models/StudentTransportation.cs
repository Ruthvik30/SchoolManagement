using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

[Table("StudentTransportation")]
public partial class StudentTransportation
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("TransportationID")]
    public int? TransportationId { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("StudentTransportation")]
    public virtual StudentDetail Student { get; set; } = null!;

    [ForeignKey("TransportationId")]
    [InverseProperty("StudentTransportations")]
    public virtual Transportation? Transportation { get; set; }
}
