using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceDescription { get; set; } = null!;

    public int ServicePrice { get; set; }

    public virtual ICollection<Servicepart> Serviceparts { get; set; } = new List<Servicepart>();
}
