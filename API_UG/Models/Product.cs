using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Product
{
    public int ProductArticle { get; set; }

    public string ProductName { get; set; } = null!;

    public int ProductCategory { get; set; }

    public string ProductDescription { get; set; } = null!;

    public int ProductPrice { get; set; }

    public byte[]? ProductPhoto { get; set; }

    public byte[]? ProductDocumentation { get; set; }

    public virtual Productcategory ProductCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Productpart> Productparts { get; set; } = new List<Productpart>();
}
