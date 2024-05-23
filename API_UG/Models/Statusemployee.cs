using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Statusemployee
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
