using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Servicepart
{
    public int ServicePartId { get; set; }

    public int ServiceId { get; set; }

    public int? AgreementId { get; set; }

    public int ServiceQuantity { get; set; }

    public int? OrderId { get; set; }

    public virtual Agreement? Agreement { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Service Service { get; set; } = null!;
}