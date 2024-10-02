using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataModels;
public class Movement
{
    public Guid Id { get; }
    public Guid IdProduct { get; set; }
    public int MovementType { get; set; } 
    public DateTime MovementDate { get; set; }
    public decimal Quantity { get; set; }
}