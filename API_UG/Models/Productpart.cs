using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Productpart
{
    public int ProductPartId { get; set; }

    public int ProductArticle { get; set; }

    public int? AgreementId { get; set; }

    public int QuantityProduct { get; set; }

    public int? OrderId { get; set; }

    public virtual Agreement? Agreement { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product ProductArticleNavigation { get; set; } = null!;
}
