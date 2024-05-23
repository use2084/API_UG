using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int OrderCompany { get; set; }

    public DateOnly OrderDate { get; set; }

    public long OrderCost { get; set; }

    public virtual Company OrderCompanyNavigation { get; set; } = null!;

    public virtual ICollection<Productpart> Productparts { get; set; } = new List<Productpart>();

    public virtual ICollection<Servicepart> Serviceparts { get; set; } = new List<Servicepart>();
}
