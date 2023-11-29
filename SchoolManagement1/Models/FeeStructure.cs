using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement1.Models;

[Table("FeeStructure")]
public partial class FeeStructure
{
    [Column("FeeID")]
    public int FeeId { get; set; }

    [Key]
    [Column("ClassID")]
    public int ClassId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? AnnualFee { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ExamFee { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("FeeStructure")]
    public virtual Class Class { get; set; } = null!;

    public decimal TotalFee()
    {
        return (decimal)(AnnualFee + ExamFee);
    }
}
