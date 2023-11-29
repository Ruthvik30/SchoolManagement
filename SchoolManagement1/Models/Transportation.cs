using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

[Table("Transportation")]
public partial class Transportation
{
    [Key]
    [Column("TransportationID")]
    public int TransportationId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RouteName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string DriverName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string DriverContact { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [InverseProperty("Transportation")]
    public virtual ICollection<StudentTransportation> StudentTransportations { get; set; } = new List<StudentTransportation>();
}
