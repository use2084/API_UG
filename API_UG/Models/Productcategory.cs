﻿using System;
using System.Collections.Generic;

namespace API_UG.Models;

public partial class Productcategory
{
    public int ProductCategoryId { get; set; }

    public string ProductCategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
